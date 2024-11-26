
namespace Backend.MoneyTransfer.Application.Common.Exceptions;

public class ForbiddenAccessException : GenericException
{
    public ForbiddenAccessException()
        : base(ApiErrorCode.ServiceForbidden, "Access Forbidden")
    {

    }
}