namespace MemoAtlas_Backend.Api.Models.DTOs.Responses;

public class MemosFromCriteriaDTO
{
    public required IEnumerable<TagWithGroupDTO> RequestedTags { get; set; }
    public required IEnumerable<PromptDTO> RequestedPrompts { get; set; }
    public required PagedResponse<MemoWithTagsAndAnswersDTO> Result { get; set; }
}
