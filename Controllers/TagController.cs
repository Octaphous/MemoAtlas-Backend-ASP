using MemoAtlas_Backend_ASP.Filters;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend_ASP.Controllers
{
    [Route("api/tags")]
    [AuthRequired]
    [ApiController]
    public class TagController(/*ITagService tagService*/) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTag(int id)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag(/*[FromBody] TagCreateBody body*/)
        {
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTag(int id/*, [FromBody] TagUpdateBody body*/)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            return Ok();
        }
    }
}