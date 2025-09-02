namespace AdminPanel.Bll.DTOs;
public class CommentResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Body { get; set; }

    public List<CommentResponseDto> ChildComments { get; set; } = new();
}