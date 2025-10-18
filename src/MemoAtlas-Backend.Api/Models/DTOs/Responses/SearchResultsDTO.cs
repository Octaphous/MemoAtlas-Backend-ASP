namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class SearchResultsDTO
{
    public required IEnumerable<MemoDTO> Memos { get; set; }
    public required IEnumerable<TagWithGroupDTO> Tags { get; set; }
    public required IEnumerable<TagGroupDTO> TagGroups { get; set; }
    public required IEnumerable<PromptDTO> Prompts { get; set; }
}
