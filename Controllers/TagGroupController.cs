using MemoAtlas_Backend_ASP.Filters;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend_ASP.Controllers;

[Route("api/tag-groups")]
[AuthRequired]
[ApiController]
public class TagGroupController(IUserContext auth, ITagGroupService tagGroupService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllTagGroups()
    {
        List<TagGroup> tagGroups = await tagGroupService.GetAllTagGroupsAsync(auth.GetRequiredUser());
        return Ok(tagGroups.Select(TagGroupMapper.ToResponse).ToList());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTagGroup(int id)
    {
        TagGroup tagGroup = await tagGroupService.GetTagGroupAsync(auth.GetRequiredUser(), id);
        return Ok(TagGroupMapper.ToResponse(tagGroup));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTagGroup([FromBody] TagGroupCreateRequest body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        TagGroup tagGroup = await tagGroupService.CreateTagGroupAsync(auth.GetRequiredUser(), body);
        return Ok(TagGroupMapper.ToResponse(tagGroup));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTagGroup(int id, [FromBody] TagGroupUpdateRequest body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        TagGroup tagGroup = await tagGroupService.UpdateTagGroupAsync(auth.GetRequiredUser(), id, body);
        return Ok(TagGroupMapper.ToResponse(tagGroup));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTagGroup(int id)
    {
        await tagGroupService.DeleteTagGroupAsync(auth.GetRequiredUser(), id);
        return Ok();
    }
}
