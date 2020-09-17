using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Collections.Generic;

namespace CurrencyRateApp.Exceptions
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilterAttribute()
        {
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(BadRequestException), HandleBadRequestException },
                { typeof(NotFoundException), HandleNotFoundException }
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }
            HandleUnknownException(context);
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            ProblemDetails details = CreateProblemDetailsObject(StatusCodes.Status500InternalServerError, 
                "https://tools.ietf.org/html/rfc7231#section-6.6.1", "Unknown exception", context.Exception.Message);
            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            context.ExceptionHandled = true;
            Log.Logger.Error(context.Exception, "Unknown exception");
            LogInnerException(context.Exception);
        }

        private static ProblemDetails CreateProblemDetailsObject(int statusCode, string type, string title, string detail)
            => new ProblemDetails
            {
                Status = statusCode,
                Type = type,
                Title = title,
                Detail = detail
            };

        private void HandleUnauthorizedAccessException(ExceptionContext context)
        {
            ProblemDetails details = CreateProblemDetailsObject(StatusCodes.Status401Unauthorized,
                "https://tools.ietf.org/html/rfc7235#section-3.1", "Unauthorized exception", string.Empty);
            context.Result = new NotFoundObjectResult(details);
            context.ExceptionHandled = true;
            Log.Logger.Error(context.Exception, "Unauthorized exception");
            LogInnerException(context.Exception);
        }

        private void HandleBadRequestException(ExceptionContext context)
        {
            var exception = context.Exception as BadRequestException;
            ProblemDetails details = CreateProblemDetailsObject(StatusCodes.Status400BadRequest,
                "https://tools.ietf.org/html/rfc7231#section-6.5.1", "Bad request", exception.Message);
            context.Result = new BadRequestObjectResult(details);
            context.ExceptionHandled = true;
            Log.Logger.Error(exception, "Bad request");
            LogInnerException(exception);
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;
            ProblemDetails details = CreateProblemDetailsObject(StatusCodes.Status404NotFound,
                "https://tools.ietf.org/html/rfc7231#section-6.5.4", "Not Found exception", exception.Message);
            context.Result = new NotFoundObjectResult(details);
            context.ExceptionHandled = true;
            Log.Logger.Error(exception, "Not Found exception");
            LogInnerException(exception);
        }

        private void LogInnerException(Exception exception)
        {
            if (default != exception.InnerException)
            {
                Log.Logger.Error(exception.InnerException, "Unknown exception");
                LogInnerException(exception.InnerException);
            }
        }
    }
}
