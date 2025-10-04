using System.Net;

namespace MemoAtlas_Backend_ASP.Exceptions
{
    public class InvalidPayloadException(string message = "Invalid payload.") : BaseException(message, HttpStatusCode.BadRequest)
    { }
}