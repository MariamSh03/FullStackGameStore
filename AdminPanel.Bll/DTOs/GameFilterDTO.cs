using System.Text.Json.Serialization;

namespace AdminPanel.Bll.DTOs;
public class GameFilterDto
{
    public string? Name { get; set; }

    public double? MinPrice { get; set; }

    public double? MaxPrice { get; set; }

    [JsonPropertyName("genres")]
    public List<Guid> GenreIds { get; set; } = new List<Guid>();

    [JsonPropertyName("platforms")]
    public List<Guid> PlatformIds { get; set; } = new List<Guid>();

    public List<string> PublisherNames { get; set; } = new List<string>();

    public string? PublishDate { get; set; }

    public int Page { get; set; } = 1;

    public int PageCount { get; set; } = 10;

    public string? Sort { get; set; }
}