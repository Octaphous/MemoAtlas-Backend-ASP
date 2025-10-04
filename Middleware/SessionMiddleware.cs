using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class SessionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, AppDbContext dbContext)
    {
        if (context.Request.Cookies.TryGetValue("SessionToken", out string? token))
        {
            Session? session = await dbContext.Sessions.Include(s => s.User)
                                                      .FirstOrDefaultAsync(s => s.SessionToken == token && s.ExpiresAt > DateTime.UtcNow);

            if (session != null)
            {
                context.Items["User"] = new UserData(session.User);
            }
        }

        await next(context);
    }
}