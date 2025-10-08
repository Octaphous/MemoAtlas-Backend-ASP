namespace MemoAtlas_Backend_ASP.Models.DTOs.Responses;

public class MemoWithTagsAndAnswersDTO
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required DateOnly Date { get; set; }
    public required IEnumerable<TagGroupWithTagsDTO> TagGroups { get; set; }
    public required IEnumerable<PromptAnswerWithPromptDTO> PromptAnswers { get; set; }
    public required bool Private { get; set; }
}
