using System.Net;

namespace MemoAtlas_Backend_ASP.Exceptions
{
    public class InvalidResourceException(string msg = "Invalid resource.") : BaseException(msg, HttpStatusCode.NotFound)
    { }
}