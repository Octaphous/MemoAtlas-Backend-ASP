using MemoAtlas_Backend_ASP.Filters;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend_ASP.Controllers
{
    [Route("api/prompts")]
    [AuthRequired]
    [ApiController]
    public class PromptController(IUserContext auth, IPromptService promptService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllPrompts()
        {
            List<PromptData> prompts = await promptService.GetAllPromptsAsync(auth.GetRequiredUser());
            return Ok(prompts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrompt(int id)
        {
            PromptData prompt = await promptService.GetPromptAsync(auth.GetRequiredUser(), id);
            return Ok(prompt);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrompt([FromBody] PromptCreateBody body)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            PromptData prompt = await promptService.CreatePromptAsync(auth.GetRequiredUser(), body);
            return Ok(prompt);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePrompt(int id, [FromBody] PromptUpdateBody body)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await promptService.UpdatePromptAsync(auth.GetRequiredUser(), id, body);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrompt(int id)
        {
            await promptService.DeletePromptAsync(auth.GetRequiredUser(), id);
            return Ok();
        }
    }
}