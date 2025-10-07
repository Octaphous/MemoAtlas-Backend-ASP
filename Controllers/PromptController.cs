using MemoAtlas_Backend_ASP.Filters;
using MemoAtlas_Backend_ASP.Mappers;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend_ASP.Controllers;

[Route("api/prompts")]
[AuthRequired]
[ApiController]
public class PromptController(IUserContext auth, IPromptService promptService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllPrompts()
    {
        IEnumerable<Prompt> prompts = await promptService.GetAllPromptsAsync(auth.GetRequiredUser());
        return Ok(prompts.Select(PromptMapper.ToDTO));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPrompt(int id)
    {
        Prompt prompt = await promptService.GetPromptAsync(auth.GetRequiredUser(), id);
        return Ok(PromptMapper.ToPromptWithMemosDTO(prompt));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrompt([FromBody] PromptCreateRequest body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        Prompt prompt = await promptService.CreatePromptAsync(auth.GetRequiredUser(), body);
        return Ok(PromptMapper.ToDTO(prompt));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdatePrompt(int id, [FromBody] PromptUpdateRequest body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        Prompt prompt = await promptService.UpdatePromptAsync(auth.GetRequiredUser(), id, body);
        return Ok(PromptMapper.ToDTO(prompt));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrompt(int id)
    {
        await promptService.DeletePromptAsync(auth.GetRequiredUser(), id);
        return Ok();
    }
}
