using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Backend.MoneyTransfer.Domain.Entities;

namespace Backend.MoneyTransfer.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(u => u.Description).HasMaxLength(1000);
        builder.Property(u => u.Status).IsRequired();
        builder.Property(u => u.Amount).IsRequired().HasPrecision(18, 2);
        builder.Property(u => u.ReceiverId).IsRequired();

        builder.Ignore(u => u.DomainEvents);
    }
}