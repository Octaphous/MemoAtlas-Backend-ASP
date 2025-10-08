using System.Net;

namespace MemoAtlas_Backend.Api.Exceptions;

public class InvalidResourceException(string msg = "Invalid resource.") : StatusCodeException(msg, HttpStatusCode.NotFound)
{ }
