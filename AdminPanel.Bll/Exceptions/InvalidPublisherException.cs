namespace AdminPanel.Bll.Exceptions;
public class InvalidPublisherException : Exception
{
    public InvalidPublisherException(string publisherId, string message)
        : base(message)
    {
        PublisherId = publisherId;
    }

    public string PublisherId { get; }
}
