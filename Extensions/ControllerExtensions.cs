using Microsoft.AspNetCore.Mvc;

public static class ControllerExtensions
{
    public static IActionResult ErrorMsg(this ControllerBase controller, string message, int statusCode = 400)
    {
        var response = new { message };
        return controller.StatusCode(statusCode, response);
    }
}