namespace AdminPanel.Bll.Exceptions;
public class OrderNotFoundException : Exception
{
    public OrderNotFoundException()
        : base("Order not found")
    {
    }

    public OrderNotFoundException(string message)
        : base(message)
    {
    }

    public OrderNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
