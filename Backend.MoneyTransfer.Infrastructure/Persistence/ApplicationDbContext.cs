using Backend.MoneyTransfer.Domain.Entities;
using System.Reflection;
using Backend.MoneyTransfer.Application.Common.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Backend.MoneyTransfer.Domain.Entities.BaseModels;
using Backend.MoneyTransfer.Domain.Commons;
using Backend.MoneyTransfer.Domain.Enums;

namespace Backend.MoneyTransfer.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Transaction> Transaction { get; set; }

    private readonly IDomainEventService _domainEventService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBus _bus;

    public ApplicationDbContext(ICurrentUserService currentUserService,
        IDomainEventService domainEventService
                            , IBus bus
                            , DbContextOptions options) : base(options)
    {
        _currentUserService = currentUserService;
        _domainEventService = domainEventService;
        _bus = bus;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.RecordStatus = RecordStatus.Active;
                    entry.Entity.CreateDate = DateTimeOffset.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdateDate = DateTimeOffset.UtcNow;
                    break;

                case EntityState.Deleted:
                    entry.Entity.RecordStatus = RecordStatus.Passive;
                    entry.Entity.UpdateDate = DateTimeOffset.UtcNow;
                    break;
            }
        }

        var events = ChangeTracker.Entries<IHasDomainEvent>()
           .Select(x => x.Entity.DomainEvents)
           .SelectMany(x => x)
           .Where(domainEvent => !domainEvent.IsPublished)
           .ToArray();

        var EntityChangeLog = PrepareChangeTrackLogs(_currentUserService.UserId);

        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents(events);

        await DispatchChangeTrackLogsAsync(EntityChangeLog);

        return result;
    }

    private List<EntityChangeLogModel> PrepareChangeTrackLogs(Guid userId)
    {
        var entries = new List<EntityChangeLogModel>();

        foreach (var entry in ChangeTracker.Entries<ITrackChange>())
        {
            var shemaName = GetSchema(entry);

            var entityChangeLog = new EntityChangeLogModel
            {
                ShemaName = shemaName,
                TableName = entry.Entity.GetType().Name,
                UserId = userId.ToString()
            };

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
            {
                entries.Add(entityChangeLog);

                var databaseValues = entry.GetDatabaseValues();

                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        entityChangeLog.KeyValues[propertyName] = property.CurrentValue?.ToString();
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entityChangeLog.CrudOperationType = CrudOperationType.Create;
                            entityChangeLog.NewValues[propertyName] = property.CurrentValue?.ToString();
                            break;

                        case EntityState.Deleted:
                            entityChangeLog.CrudOperationType = CrudOperationType.Delete;
                            entityChangeLog.OldValues[propertyName] = property.OriginalValue?.ToString();
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                entityChangeLog.AffectedColumns.Add(propertyName);
                                entityChangeLog.CrudOperationType = CrudOperationType.Update;
                                entityChangeLog.OldValues[propertyName] = databaseValues[propertyName]?.ToString();
                                entityChangeLog.NewValues[propertyName] = property.CurrentValue?.ToString();
                            }
                            break;
                    }
                }
            }
        }
        return entries;
    }

    private async Task DispatchChangeTrackLogsAsync(List<EntityChangeLogModel> entityChangeLogList)
    {
        foreach (var item in entityChangeLogList)
        {
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var busEndpoint = await _bus.GetSendEndpoint(new Uri("exchange:Log.ChangeTracker"));
            await busEndpoint.Send(item, cancellationToken.Token);
        }
    }

    private string GetSchema(EntityEntry entry)
    {
        var entity = entry.Entity;
        var schemaAnnotation = base.Model.FindEntityType(entity.GetType()).GetAnnotations()
        .FirstOrDefault(a => a.Name == "Relational:Schema");
        return schemaAnnotation == null ? "dbo" : schemaAnnotation.Value.ToString();
    }

    private async Task DispatchEvents(DomainEvent[] events)
    {
        foreach (var @event in events)
        {
            @event.IsPublished = true;
            await _domainEventService.PublishAsync(@event);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);

        SetTableNames(builder);

        ConvertEnumColumnsToString(builder);
    }

    private static void SetTableNames(ModelBuilder builder)
    {
        builder.Entity<User>().ToTable("User", "Identity");
        builder.Entity<IdentityRole<Guid>>().ToTable("Role", "Identity");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaim", "Identity");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserToken", "Identity");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRole", "Identity");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaim", "Identity");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogin", "Identity");
    }

    private static void ConvertEnumColumnsToString(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType.IsEnum)
                {
                    var type = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
                    var converter = Activator.CreateInstance(type, new ConverterMappingHints()) as ValueConverter;

                    property.SetValueConverter(converter);
                    property.SetMaxLength(50);
                }
            }
        }
    }
}