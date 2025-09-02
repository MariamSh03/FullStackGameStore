namespace AdminPanel.Bll.Exceptions;
public class InvalidPlatformsException : Exception
{
    public InvalidPlatformsException(IEnumerable<Guid> invalidPlatformIds, string message)
        : base(message)
    {
        InvalidPlatformIds = invalidPlatformIds;
    }

    public IEnumerable<Guid> InvalidPlatformIds { get; }
}