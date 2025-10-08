using System.Net;

namespace MemoAtlas_Backend.Api.Exceptions;

public class InvalidPayloadException(string message = "Invalid payload.") : StatusCodeException(message, HttpStatusCode.BadRequest)
{ }
