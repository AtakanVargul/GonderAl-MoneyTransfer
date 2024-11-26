namespace Backend.MoneyTransfer.Domain.Entities.BaseModels;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}