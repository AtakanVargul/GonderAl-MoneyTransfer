using Backend.MoneyTransfer.Domain.Enums;

namespace Backend.MoneyTransfer.Domain.Entities.BaseModels;

public class EntityChangeLogModel
{
    public string ShemaName { get; set; }
    public string TableName { get; set; }
    public CrudOperationType CrudOperationType { get; set; }
    public string UserId { get; set; }
    public Dictionary<string, string> KeyValues { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, string> OldValues { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, string> NewValues { get; set; } = new Dictionary<string, string>();
    public List<string> AffectedColumns { get; set; } = new List<string>();
}