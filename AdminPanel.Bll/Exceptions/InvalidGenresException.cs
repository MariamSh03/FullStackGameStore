namespace AdminPanel.Bll.Exceptions;
public class InvalidGenresException : Exception
{
    public InvalidGenresException(IEnumerable<Guid> invalidGenreIds, string message)
        : base(message)
    {
        InvalidGenreIds = invalidGenreIds;
    }

    public IEnumerable<Guid> InvalidGenreIds { get; }
}
