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
        List<TagGroupResponse> tagGroups = await tagGroupService.GetAllTagGroupsAsync(auth.GetRequiredUser());
        return Ok(tagGroups);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTagGroup(int id)
    {
        TagGroupResponse tagGroup = await tagGroupService.GetTagGroupAsync(auth.GetRequiredUser(), id);
        return Ok(tagGroup);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTagGroup([FromBody] TagGroupCreateRequest body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        TagGroupResponse tagGroup = await tagGroupService.CreateTagGroupAsync(auth.GetRequiredUser(), body);
        return Ok(tagGroup);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTagGroup(int id, [FromBody] TagGroupUpdateRequest body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await tagGroupService.UpdateTagGroupAsync(auth.GetRequiredUser(), id, body);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTagGroup(int id)
    {
        await tagGroupService.DeleteTagGroupAsync(auth.GetRequiredUser(), id);
        return Ok();
    }
}
