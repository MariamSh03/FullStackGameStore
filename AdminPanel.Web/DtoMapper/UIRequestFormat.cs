namespace AdminPanel.Web.DtoMapper;

public class UIRequestFormat
{
    public GameDetails Game { get; set; }

    public List<string> Genres { get; set; }

    public List<string> Platforms { get; set; }

    public string Publisher { get; set; }
}