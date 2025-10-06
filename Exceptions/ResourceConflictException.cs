using System.Net;

namespace MemoAtlas_Backend_ASP.Exceptions;

public class ResourceConflictException(string msg = "This resource already exists.") : StatusCodeException(msg, HttpStatusCode.Conflict)
{ }
