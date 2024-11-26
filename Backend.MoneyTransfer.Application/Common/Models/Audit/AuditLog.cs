namespace Backend.MoneyTransfer.Application.Common.Models.Audit;

public class AuditLog
{
    public DateTimeOffset LogDate { get; set; }
    public string SourceApplication { get; set; }
    public string Resource { get; set; }
    public string Operation { get; set; }
    public string UserName { get; set; }
    public Guid UserId { get; set; }
    public bool IsSuccess { get; set; }
    public Dictionary<string, string> Details { get; set; }
}