using System.Net;
using System.Text.Json;
using Uber.Helpers.Exceptions.GeneralExceptions;
using KeyNotFoundException = System.Collections.Generic.KeyNotFoundException;
using NotImplementedException = System.NotImplementedException;

namespace Uber.MiddleWares;

public class GlobalExceptionHandlingMiddleWare
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddleWare(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext,ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        HttpStatusCode status;
        var stackTrace = string.Empty;
        string message = "";

        var exceptionType = exception.GetType();
        if (exceptionType == typeof(NotFoundException))
        {
            message = exception.Message;
            status = HttpStatusCode.NotFound;
            stackTrace = exception.StackTrace;
        }
        else if (exceptionType == typeof(BadRequestException))
        {
            message = exception.Message;
            status = HttpStatusCode.BadRequest;
            stackTrace = exception.StackTrace;
        }
        else if (exceptionType == typeof(NotImplementedException))
        {
            message = exception.Message;
            status = HttpStatusCode.NotImplemented;
            stackTrace = exception.StackTrace;
        }
        else if (exceptionType == typeof(UnauthorizedAccessException))
        {
            message = exception.Message;
            status = HttpStatusCode.Unauthorized;
            stackTrace = exception.StackTrace;
        }
        else if (exceptionType == typeof(KeyNotFoundException))
        {
            message = exception.Message;
            status = HttpStatusCode.NotFound;
            stackTrace = exception.StackTrace;
        }
        else
        {
            message = exception.Message;
            status = HttpStatusCode.InternalServerError;
            stackTrace = exception.StackTrace;
        }
        var exceptionResult = JsonSerializer.Serialize(new { error = message, stackTrace });
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)status;
        return httpContext.Response.WriteAsync(exceptionResult);
    }
}