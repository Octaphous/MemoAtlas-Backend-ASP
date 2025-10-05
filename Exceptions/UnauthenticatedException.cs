using System.Net;

namespace MemoAtlas_Backend_ASP.Exceptions
{
    public class UnauthenticatedException(string msg = "User is not authenticated.") : StatusCodeException(msg, HttpStatusCode.Unauthorized)
    { }
}