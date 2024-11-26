﻿using Backend.MoneyTransfer.Application.Common.Exceptions;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Backend.MoneyTransfer.Application.Common.Filters.ExceptionFilter;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    private readonly IStringLocalizer _localizer;
    private readonly ILogger<ApiExceptionFilterAttribute> _logger;

    public ApiExceptionFilterAttribute(IStringLocalizerFactory localizerFactory,
        ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ApiExceptionFilterAttribute>();
        _localizer = localizerFactory.Create("Exceptions", Assembly.GetEntryAssembly()!.FullName);

        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(ApiException), HandleApiException },
            { typeof(GenericException), HandleGenericException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            { typeof(NotFoundException), HandleNotFoundException },
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        var baseType = type.BaseType;

        if (baseType != null && _exceptionHandlers.ContainsKey(baseType))
        {
            _exceptionHandlers[baseType].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleApiException(ExceptionContext context)
    {
        var exception = (ApiException)context.Exception;

        var details = new ProblemDetails()
        {
            Detail = _localizer.GetString(exception.GetType().Name)
        };

        details.Extensions.Add("code", exception.Code);

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;

        _logger.LogError("ApiException(400) : {message}", exception);
    }

    private void HandleGenericException(ExceptionContext context)
    {
        var exception = (GenericException)context.Exception;

        var details = new ProblemDetails()
        {
            Detail = exception.Message
        };

        details.Extensions.Add("code", exception.Code);

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;

        _logger.LogError("GenericException (400) : Code: {code} {message}",
            exception.Code, exception);
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;

        var details = new ValidationProblemDetails(exception.Errors)
        {
            Title = "Validation Error",
            Detail = exception.Message
        };

        details.Extensions.Add("code", ApiErrorCode.ValidationError);

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;

        _logger.LogError("ValidationException(400) : Code: {code} {message}",
            exception.Code, exception);
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;

        var details = new ProblemDetails()
        {
            Title = "Resource NotFound",
            Detail = exception.Message
        };

        details.Extensions.Add("code", exception.Code);

        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;

        _logger.LogError("NotFoundObjectResult(400) : Code: {code} {message}",
            exception.Code, exception);
    }

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState)
        {
            Title = "ModelInvalid",
            Detail = "Invalid parameter(s)"
        };

        details.Extensions.Add("code", ApiErrorCode.InvalidParameters);

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;

        _logger.LogError("InvalidModelStateException(400) : Code: {code} {message}",
            ApiErrorCode.InvalidParameters, context?.Exception);
    }

    private void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };

        context.ExceptionHandled = true;

        _logger.LogError("Status401Unauthorized : {message}", context?.Exception);
    }

    private void HandleForbiddenAccessException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Forbidden",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status403Forbidden
        };

        context.ExceptionHandled = true;

        _logger.LogError("Status403Forbidden : {message}", context?.Exception);
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
        };

        details.Extensions.Add("code", ApiErrorCode.InternalError);

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;

        _logger.LogError("Status500InternalServerError : {message}", context?.Exception);
    }
}