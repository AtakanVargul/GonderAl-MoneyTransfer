using Backend.MoneyTransfer.Application.Common.Models.Audit;

namespace Backend.MoneyTransfer.Application.Common.Interfaces;

public interface IAuditLogService
{
    public Task AuditLogAsync(AuditLog auditLog);
}