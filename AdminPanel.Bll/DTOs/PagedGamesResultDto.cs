namespace AdminPanel.Bll.DTOs;
public class PagedGamesResultDto
{
    public IEnumerable<GameResponseDto> Games { get; set; }

    public int TotalPages { get; set; }

    public int CurrentPage { get; set; }

    public int TotalCount { get; set; }
}