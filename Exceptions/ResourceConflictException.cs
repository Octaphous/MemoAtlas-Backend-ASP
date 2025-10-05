using System.Net;

namespace MemoAtlas_Backend_ASP.Exceptions
{
    public class ResourceConflictException(string msg = "This resource already exists.") : BaseException(msg, HttpStatusCode.Conflict)
    { }
}