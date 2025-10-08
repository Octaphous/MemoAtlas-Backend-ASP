using MemoAtlas_Backend.Api.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MemoAtlas_Backend.Api.Filters;

public class AuthRequiredAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Items["User"] == null)
        {
            throw new UnauthenticatedException();
        }
    }
}
