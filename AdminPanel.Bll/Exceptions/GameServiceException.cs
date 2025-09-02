namespace AdminPanel.Bll.Exceptions;
public class GameServiceException : Exception
{
    // Constructor with message and inner exception
    public GameServiceException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    // Constructor with just a message
    public GameServiceException(string message)
        : base(message)
    {
    }
}