using MassTransit;
using Microsoft.Extensions.Logging;
using Backend.MoneyTransfer.Application.Common.Interfaces;

namespace Backend.MoneyTransfer.Application.Common.Models.Audit;

public class AuditLogService : IAuditLogService
{
    private readonly IBus _bus;
    private readonly ILogger<AuditLogService> _logger;

    public AuditLogService(IBus bus, ILogger<AuditLogService> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task AuditLogAsync(AuditLog auditLog)
    {
        try
        {
            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var endpoint = await _bus.GetSendEndpoint(new Uri("exchange:Log.AuditLog"));
            await endpoint.Send(auditLog, tokenSource.Token);
        }
        catch (System.Exception exception)
        {
            _logger.LogError($"ExceptionOnSendAuditLog detail:\n{exception}");
        }
    }
}