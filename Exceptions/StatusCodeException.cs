using System.Net;

namespace MemoAtlas_Backend_ASP.Exceptions;

public class StatusCodeException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}
