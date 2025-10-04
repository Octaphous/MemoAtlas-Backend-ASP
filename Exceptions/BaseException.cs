using System.Net;

namespace MemoAtlas_Backend_ASP.Exceptions
{
    public class BaseException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; } = statusCode;
    }
}