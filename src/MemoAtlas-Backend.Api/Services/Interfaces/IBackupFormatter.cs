using MemoAtlas_Backend.Api.Models.DTOs.Responses;

namespace MemoAtlas_Backend.Api.Services.Interfaces;

public interface IBackupFormatter
{
    string ToMarkdown(IEnumerable<MemoWithTagsAndAnswersDTO> memos);
}