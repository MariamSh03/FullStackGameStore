namespace AdminPanel.Bll.DTOs;
public class OrderDto
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public DateTime? Date { get; set; }
}