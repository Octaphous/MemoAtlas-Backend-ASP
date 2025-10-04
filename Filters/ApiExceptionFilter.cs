using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace MemoAtlas_Backend_ASP.Filters
{
    public class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, "Unhandled exception occurred");

            var (statusCode, message) = context.Exception switch
            {
                InvalidOperationException ex => (HttpStatusCode.BadRequest, ex.Message),
                UnauthorizedAccessException ex => (HttpStatusCode.Forbidden, ex.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
            };

            ProblemDetails problem = new()
            {
                Status = (int)statusCode,
                Title = message,
                Type = $"https://httpstatuses.com/{(int)statusCode}"
            };

            context.Result = new ObjectResult(problem)
            {
                StatusCode = (int)statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}