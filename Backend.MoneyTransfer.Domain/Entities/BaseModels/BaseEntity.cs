namespace Backend.MoneyTransfer.Domain.Entities.BaseModels;

public class BaseEntity : IEntity<Guid>
{
    public Guid Id { get; set; } = SequentialGuid.NewSequentialGuid();
}