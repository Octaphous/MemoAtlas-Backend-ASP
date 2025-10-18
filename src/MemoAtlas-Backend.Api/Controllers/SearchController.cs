using MemoAtlas_Backend.Api.Filters;
using MemoAtlas_Backend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;

namespace MemoAtlas_Backend.Api.Controllers;

[Route("api/search")]
[AuthRequired]
[ApiController]
public class SearchController(IUserContext auth, ISearchService searchService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        SearchResultsDTO results = await searchService.SearchAll(auth.GetRequiredUser(), query);
        return Ok(results);
    }
}