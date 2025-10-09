using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models.Configurations;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace MemoAtlas_Backend.Api.Middleware;

public class SessionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, ISessionService sessionService, IOptions<AuthOptions> authOptions)
    {
        if (context.Request.Cookies.TryGetValue(authOptions.Value.SessionTokenName, out string? token))
        {
            try
            {
                Session session = await sessionService.GetSessionByTokenAsync(token);
                context.Items["User"] = session.User;
            }
            catch (InvalidResourceException)
            {
                context.Items["User"] = null;
            }
        }

        await next(context);
    }
}
