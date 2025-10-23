using MemoAtlas_Backend.Api.Data;
using MemoAtlas_Backend.Api.Mappers;
using MemoAtlas_Backend.Api.Models.DTOs.Backup;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Repositories.Interfaces;
using MemoAtlas_Backend.Api.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MemoAtlas_Backend.Api.Repositories;

public class BackupRepository(AppDbContext db) : IBackupRepository
{
    public async Task<FullBackupV1> CreateFullBackupAsync(User user)
    {
        List<Memo> memos = await db.Memos
            .VisibleToUser(user)
            .Where(m => m.UserId == user.Id)
            .Include(m => m.Tags)
            .ToListAsync();

        List<TagGroup> tagGroups = await db.TagGroups
            .VisibleToUser(user)
            .Where(tg => tg.UserId == user.Id)
            .ToListAsync();

        List<Tag> tags = await db.Tags
            .VisibleToUser(user)
            .Where(t => t.TagGroup.UserId == user.Id)
            .ToListAsync();

        List<Prompt> prompts = await db.Prompts
            .VisibleToUser(user)
            .Where(p => p.UserId == user.Id)
            .ToListAsync();

        List<PromptAnswerText> promptAnswerTexts = await db.PromptAnswerTexts
            .VisibleToUser(user)
            .Where(pa => pa.Memo.UserId == user.Id)
            .Include(pa => pa.Memo)
            .Include(pa => pa.Prompt)
            .ToListAsync();

        List<PromptAnswerNumber> promptAnswerNumbers = await db.PromptAnswerNumbers
            .VisibleToUser(user)
            .Where(pa => pa.Memo.UserId == user.Id)
            .Include(pa => pa.Memo)
            .Include(pa => pa.Prompt)
            .ToListAsync();

        if (!user.PrivateMode)
        {
            static bool filter(PromptAnswer pa) => !pa.Prompt.Private && !pa.Memo.Private;
            promptAnswerTexts = promptAnswerTexts.Where(pa => filter(pa)).ToList();
            promptAnswerNumbers = promptAnswerNumbers.Where(pa => filter(pa)).ToList();
        }

        List<MemoTagBackupV1> memoTags = [];

        foreach (Memo memo in memos)
        {
            foreach (Tag tag in memo.Tags)
            {
                memoTags.Add(new MemoTagBackupV1
                {
                    MemoId = memo.Id,
                    TagId = tag.Id
                });
            }
        }

        return new FullBackupV1
        {
            Memos = memos.Select(BackupMapper.Memo).ToList(),
            TagGroups = tagGroups.Select(BackupMapper.TagGroup).ToList(),
            Tags = tags.Select(BackupMapper.Tag).ToList(),
            MemoTags = memoTags,
            Prompts = prompts.Select(BackupMapper.Prompt).ToList(),
            PromptAnswerTexts = promptAnswerTexts.Select(BackupMapper.PromptAnswerText).ToList(),
            PromptAnswerNumbers = promptAnswerNumbers.Select(BackupMapper.PromptAnswerNumber).ToList()
        };
    }

    public async Task TryFullBackupRestoreAsync(User user, FullBackupV1 backup)
    {
        using IDbContextTransaction transaction = await db.Database.BeginTransactionAsync();
        try
        {
            await RestoreFullBackupAsync(user, backup);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task RestoreFullBackupAsync(User user, FullBackupV1 backup)
    {
        // Delete existing data
        var userMemos = db.Memos.Where(m => m.UserId == user.Id);
        db.Memos.RemoveRange(userMemos);

        var userTagGroups = db.TagGroups.Where(tg => tg.UserId == user.Id);
        db.TagGroups.RemoveRange(userTagGroups);

        var userTags = db.Tags.Where(t => t.TagGroup.UserId == user.Id);
        db.Tags.RemoveRange(userTags);

        var userPrompts = db.Prompts.Where(p => p.UserId == user.Id);
        db.Prompts.RemoveRange(userPrompts);

        await db.SaveChangesAsync();

        // Dictionaries to map old IDs to new IDs
        Dictionary<int, int> tagGroupIds = [];
        Dictionary<int, int> memoIds = [];
        Dictionary<int, int> promptIds = [];
        Dictionary<int, int> tagIds = [];

        // Lists to hold our new entities
        List<TagGroup> newTagGroups = [];
        List<Prompt> newPrompts = [];
        List<Memo> newMemos = [];
        List<Tag> newTags = [];
        List<PromptAnswer> newPromptAnswers = [];

        // Restore tag groups
        foreach (TagGroupBackupV1 tg in backup.TagGroups)
        {
            newTagGroups.Add(new()
            {
                UserId = user.Id,
                Name = tg.Name,
                Color = tg.Color,
                Private = tg.Private
            });
        }
        db.TagGroups.AddRange(newTagGroups);

        // Restore prompts
        foreach (PromptBackupV1 p in backup.Prompts)
        {
            newPrompts.Add(new()
            {
                UserId = user.Id,
                Question = p.Question,
                Type = p.Type,
                Private = p.Private
            });
        }
        db.Prompts.AddRange(newPrompts);

        // Restore memos
        foreach (MemoBackupV1 m in backup.Memos)
        {
            newMemos.Add(new()
            {
                UserId = user.Id,
                Title = m.Title,
                Date = m.Date,
                Private = m.Private
            });
        }
        db.Memos.AddRange(newMemos);

        // Save to give the new tag groups, prompts, and memos their new IDs
        await db.SaveChangesAsync();

        // Map old IDs to new IDs
        for (int i = 0; i < newTagGroups.Count; i++)
        {
            tagGroupIds[backup.TagGroups[i].Id] = newTagGroups[i].Id;
        }

        for (int i = 0; i < newPrompts.Count; i++)
        {
            promptIds[backup.Prompts[i].Id] = newPrompts[i].Id;
        }

        for (int i = 0; i < newMemos.Count; i++)
        {
            memoIds[backup.Memos[i].Id] = newMemos[i].Id;
        }

        // Now when the tag groups have got their new IDs, we can restore tags
        foreach (TagBackupV1 t in backup.Tags)
        {
            newTags.Add(new()
            {
                Name = t.Name,
                Description = t.Description,
                TagGroupId = tagGroupIds[t.TagGroupId],
                Private = t.Private
            });
        }
        db.Tags.AddRange(newTags);

        // Restore number prompt answers
        foreach (PromptAnswerNumberBackupV1 pan in backup.PromptAnswerNumbers)
        {
            newPromptAnswers.Add(new PromptAnswerNumber
            {
                MemoId = memoIds[pan.MemoId],
                PromptId = promptIds[pan.PromptId],
                Number = pan.Number,
                Private = pan.Private
            });
        }

        // Restore text prompt answers
        foreach (PromptAnswerTextBackupV1 pat in backup.PromptAnswerTexts)
        {
            newPromptAnswers.Add(new PromptAnswerText
            {
                MemoId = memoIds[pat.MemoId],
                PromptId = promptIds[pat.PromptId],
                Text = pat.Text,
                Private = pat.Private
            });
        }
        db.PromptAnswers.AddRange(newPromptAnswers);

        // Save to give the new tags and prompt answers their new IDs
        await db.SaveChangesAsync();

        // Map old tag IDs to new tag IDs
        for (int i = 0; i < newTags.Count; i++)
        {
            tagIds[backup.Tags[i].Id] = newTags[i].Id;
        }

        // Now when the new tags and new memos have their new IDs, we can restore memo-tag relationships
        Dictionary<int, Memo> memoDict = newMemos.ToDictionary(m => m.Id);
        Dictionary<int, Tag> tagDict = newTags.ToDictionary(t => t.Id);
        foreach (MemoTagBackupV1 mt in backup.MemoTags)
        {
            Memo? memo = memoDict[memoIds[mt.MemoId]];
            Tag? tag = tagDict[tagIds[mt.TagId]];

            if (memo != null && tag != null)
            {
                memo.Tags.Add(tag);
            }
        }

        await db.SaveChangesAsync();
    }
}