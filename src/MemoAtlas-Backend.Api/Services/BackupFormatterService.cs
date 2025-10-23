using System.Text;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Services.Interfaces;

namespace MemoAtlas_Backend.Api.Services;

public class BackupFormatterService : IBackupFormatter
{
    public string ToMarkdown(IEnumerable<MemoWithTagsAndAnswersDTO> memos)
    {
        StringBuilder sb = new();

        foreach (MemoWithTagsAndAnswersDTO memo in memos)
        {
            sb.AppendLine($"# {memo.Date} - {memo.Title}");

            sb.AppendLine("\n## Tags");
            foreach (TagGroupWithTagsDTO tagGroup in memo.TagGroups)
            {
                IEnumerable<string> tagNames = tagGroup.Tags.Select(t => t.Name);
                sb.AppendLine($"- **{tagGroup.Name}**: {string.Join(", ", tagNames)}");
            }

            sb.AppendLine("\n## Questions and Answers");
            foreach (PromptWithAnswerDTO prompt in memo.Prompts)
            {
                IEnumerable<string> answers = prompt.Answers.Select(answer =>
                {
                    return answer switch
                    {
                        PromptAnswerTextDTO textAnswer => textAnswer.Text,
                        PromptAnswerNumberDTO numberAnswer => numberAnswer.Number.ToString(),
                        _ => string.Empty
                    };
                });

                sb.AppendLine($"**{prompt.Question}**");
                foreach (string answer in answers)
                {
                    sb.AppendLine($"- {answer}\n");
                }
            }

            sb.AppendLine("\n---\n");
        }

        return sb.ToString();
    }
}