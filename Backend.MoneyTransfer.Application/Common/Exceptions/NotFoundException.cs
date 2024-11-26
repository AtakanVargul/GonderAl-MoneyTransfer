
namespace Backend.MoneyTransfer.Application.Common.Exceptions;

public class NotFoundException : GenericException
{
    public NotFoundException(string message)
        : base(ApiErrorCode.NotFound, message)
    {
    }

    public NotFoundException(string name, object key)
        : base(ApiErrorCode.NotFound, $"Entity {name} ({key}) was not found.")
    {
    }
}