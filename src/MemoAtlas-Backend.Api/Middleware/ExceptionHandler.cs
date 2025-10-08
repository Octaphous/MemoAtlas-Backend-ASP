
using MemoAtlas_Backend.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend.Api.Middleware;

public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
{

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {

        ProblemDetails problemDetails = new()
        {
            Instance = httpContext.Request.Path
        };

        if (exception is StatusCodeException e)
        {
            httpContext.Response.StatusCode = (int)e.StatusCode;
            problemDetails.Title = e.Message;
        }
        else
        {
            httpContext.Response.StatusCode = 500;
            problemDetails.Title = exception.Message;
        }

        logger.LogError("{Message}", problemDetails.Title);
        problemDetails.Status = httpContext.Response.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
        return true;
    }
}
