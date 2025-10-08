using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Services.Interfaces;

namespace MemoAtlas_Backend.Api.Middleware;

public class SessionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, ISessionService sessionService)
    {
        if (context.Request.Cookies.TryGetValue(AppConstants.AuthTokenName, out string? token))
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
