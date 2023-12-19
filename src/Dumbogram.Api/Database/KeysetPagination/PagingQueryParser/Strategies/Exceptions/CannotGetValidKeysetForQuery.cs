namespace Dumbogram.Api.Database.KeysetPagination.PagingQueryParser.Strategies.Exceptions;

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