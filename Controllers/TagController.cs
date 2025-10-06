using MemoAtlas_Backend_ASP.Filters;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend_ASP.Controllers;

[Route("api/tags")]
[AuthRequired]
[ApiController]
public class TagController(IUserContext auth, ITagService tagService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllTags()
    {
        List<Tag> tags = await tagService.GetAllTagsAsync(auth.GetRequiredUser());
        return Ok(tags.Select(TagMapper.ToDetailedResponse).ToList());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTag(int id)
    {
        Tag tag = await tagService.GetTagAsync(auth.GetRequiredUser(), id);
        return Ok(TagMapper.ToDetailedResponse(tag));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] TagCreateRequest body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        Tag tag = await tagService.CreateTagAsync(auth.GetRequiredUser(), body);
        return Ok(TagMapper.ToDetailedResponse(tag));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTag(int id, [FromBody] TagUpdateRequest body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        Tag tag = await tagService.UpdateTagAsync(auth.GetRequiredUser(), id, body);
        return Ok(TagMapper.ToDetailedResponse(tag));
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(int id)
    {
        await tagService.DeleteTagAsync(auth.GetRequiredUser(), id);
        return Ok();
    }
}
