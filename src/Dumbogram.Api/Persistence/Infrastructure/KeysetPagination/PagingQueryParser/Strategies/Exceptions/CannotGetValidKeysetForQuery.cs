namespace Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.PagingQueryParser.Strategies.Exceptions;

public class CannotGetValidKeysetForQuery : ApplicationException
{
    public CannotGetValidKeysetForQuery()
    {
    }

    public CannotGetValidKeysetForQuery(string message)
        : base(message)
    {
    }
}