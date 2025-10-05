using System.Net;

namespace MemoAtlas_Backend_ASP.Exceptions
{
    public class ForbiddenResourceException(string msg = "You do not have permission to access this resource.") : StatusCodeException(msg, HttpStatusCode.Forbidden)
    { }
}