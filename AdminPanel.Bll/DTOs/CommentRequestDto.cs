namespace AdminPanel.Bll.DTOs;
public class CommentRequestDto
{
    public CommentContentDto Comment { get; set; }

    public Guid? ParentId { get; set; }

    public string? Action { get; set; }
}