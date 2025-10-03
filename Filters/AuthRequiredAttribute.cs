using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MemoAtlas_Backend_ASP.Filters
{
    public class AuthRequiredAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Items["User"] == null)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}