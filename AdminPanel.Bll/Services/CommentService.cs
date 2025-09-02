using System.Collections.Concurrent;
using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Exceptions;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Dal.Repositories;
using AdminPanel.Entity;

namespace AdminPanel.Bll.Services;

public class CommentService : ICommentService
{
    private readonly IGenericRepository<CommentEntity> _commentRepository;
    private readonly IGameRepository _gameRepository;
    private readonly ConcurrentDictionary<string, DateTime> _bannedUsers = new();

    public CommentService(IGenericRepository<CommentEntity> commentRepository, IGameRepository gameRepository)
    {
        _commentRepository = commentRepository;
        _gameRepository = gameRepository;
    }

    public async Task AddCommentAsync(string gameKey, string name, string body, Guid? parentId)
    {
        if (await IsUserBannedAsync(name))
        {
            throw new GameServiceException($"User '{name}' is currently banned from commenting.");
        }

        var game = (await _gameRepository.FindAsync(g => g.Key == gameKey)).FirstOrDefault()
                   ?? throw new GameNotFoundException("Game not found.");

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(body))
        {
            throw new GameServiceException("Name and body are required.");
        }

        var comment = new CommentEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
            Body = body,
            GameId = game.Id,
            ParentCommentId = parentId,
        };

        await _commentRepository.AddAsync(comment);
    }

    public async Task<IEnumerable<CommentResponseDto>> GetCommentsAsync(string gameKey)
    {
        var game = (await _gameRepository.FindAsync(g => g.Key == gameKey)).FirstOrDefault()
                   ?? throw new GameNotFoundException("Game not found.");

        var allComments = await _commentRepository.FindAsync(c => c.GameId == game.Id);

        var commentDict = allComments.ToDictionary(c => c.Id, c =>
            new CommentResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Body = c.Body,
                ChildComments = new List<CommentResponseDto>(),
            });

        foreach (var comment in commentDict.Values)
        {
            if (allComments.First(c => c.Id == comment.Id).ParentCommentId is Guid parentId &&
                commentDict.ContainsKey(parentId))
            {
                commentDict[parentId].ChildComments.Add(comment);
            }
        }

        return commentDict.Values
            .Where(c => allComments.First(ac => ac.Id == c.Id).ParentCommentId == null)
            .ToList();
    }

    public async Task DeleteCommentAsync(Guid commentId)
    {
        var comment = await _commentRepository.GetByIdAsync(commentId)
                      ?? throw new GameNotFoundException("Comment not found.");

        await _commentRepository.DeleteAsync(comment);
    }

    public async Task BanUserAsync(string user, string duration)
    {
        var expiration = duration.ToLower() switch
        {
            "1 hour" => DateTime.UtcNow.AddHours(1),
            "1 day" => DateTime.UtcNow.AddDays(1),
            "1 week" => DateTime.UtcNow.AddDays(7),
            "1 month" => DateTime.UtcNow.AddMonths(1),
            "permanent" => DateTime.MaxValue,
            _ => throw new GameServiceException("Invalid ban duration."),
        };

        _bannedUsers[user.ToLower()] = expiration;
        await Task.CompletedTask;
    }

    public async Task<IEnumerable<string>> GetBanDurationsAsync()
    {
        return await Task.FromResult(new List<string>
        {
            "1 hour", "1 day", "1 week", "1 month", "permanent",
        });
    }

    private async Task<bool> IsUserBannedAsync(string user)
    {
        if (_bannedUsers.TryGetValue(user.ToLower(), out var bannedUntil))
        {
            if (bannedUntil > DateTime.UtcNow)
            {
                return true;
            }

            // Ban expired, remove it
            _bannedUsers.TryRemove(user.ToLower(), out _);
        }

        return await Task.FromResult(false);
    }
}