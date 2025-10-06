using MemoAtlas_Backend_ASP.Filters;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Bodies;
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
        List<TagData> tags = await tagService.GetAllTagsAsync(auth.GetRequiredUser());
        return Ok(tags);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTag(int id)
    {
        TagData tag = await tagService.GetTagAsync(auth.GetRequiredUser(), id);
        return Ok(tag);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] TagCreateBody body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        TagData tag = await tagService.CreateTagAsync(auth.GetRequiredUser(), body);
        return Ok(tag);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTag(int id, [FromBody] TagUpdateBody body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await tagService.UpdateTagAsync(auth.GetRequiredUser(), id, body);
        return Ok();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(int id)
    {
        await tagService.DeleteTagAsync(auth.GetRequiredUser(), id);
        return Ok();
    }
}
