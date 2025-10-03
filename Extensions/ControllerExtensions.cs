using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend_ASP.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ErrorMsg(this ControllerBase controller, string message, int statusCode = 400)
        {
            var response = new { message };
            return controller.StatusCode(statusCode, response);
        }
    }
}