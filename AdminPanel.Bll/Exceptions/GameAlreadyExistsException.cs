namespace AdminPanel.Bll.Exceptions;
public class GameAlreadyExistsException : Exception
{
    public GameAlreadyExistsException(string gameKey, string message)
        : base(message)
    {
        GameKey = gameKey;
    }

    public string GameKey { get; }
}