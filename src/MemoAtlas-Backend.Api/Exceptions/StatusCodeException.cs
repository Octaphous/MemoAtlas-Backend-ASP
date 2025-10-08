using System.Net;

namespace MemoAtlas_Backend.Api.Exceptions;

public class StatusCodeException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}
