using AdminPanel.Bll.DTOs;

namespace AdminPanel.Bll.Interfaces;
public interface ICommentService
{
    Task AddCommentAsync(string gameKey, string name, string body, Guid? parentId);

    Task<IEnumerable<CommentResponseDto>> GetCommentsAsync(string gameKey);

    Task DeleteCommentAsync(Guid commentId);

    Task BanUserAsync(string user, string duration);

    Task<IEnumerable<string>> GetBanDurationsAsync();
}
