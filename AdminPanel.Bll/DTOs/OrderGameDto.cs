namespace AdminPanel.Bll.DTOs;
public class OrderGameDto
{
    public Guid GameId { get; set; }

    public string GameName { get; set; }

    public double Price { get; set; }

    public int Quantity { get; set; }
}
