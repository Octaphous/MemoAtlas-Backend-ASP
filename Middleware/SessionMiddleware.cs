using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Exceptions;
using MemoAtlas_Backend_ASP.Models;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Middleware;

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
