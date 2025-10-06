using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Models;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemoAtlas_Backend_ASP.Middleware;

public class SessionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, AppDbContext dbContext)
    {
        if (context.Request.Cookies.TryGetValue(AppConstants.AuthTokenName, out string? token))
        {
            Session? session = await dbContext.Sessions.Include(s => s.User)
                                                      .FirstOrDefaultAsync(s => s.Token == token && s.ExpiresAt > DateTime.UtcNow);

            if (session != null)
            {
                context.Items["User"] = new UserData(session.User);
            }
        }

        await next(context);
    }
}
