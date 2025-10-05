using MemoAtlas_Backend_ASP.Filters;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend_ASP.Controllers
{
    [Route("api/prompts")]
    [AuthRequired]
    [ApiController]
    public class PromptController(/*IPromptService promptService*/) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllPrompts()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrompt(int id)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrompt(/*[FromBody] PromptCreateBody body*/)
        {
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePrompt(int id/*, [FromBody] PromptUpdateBody body*/)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrompt(int id)
        {
            return Ok();
        }
    }
}