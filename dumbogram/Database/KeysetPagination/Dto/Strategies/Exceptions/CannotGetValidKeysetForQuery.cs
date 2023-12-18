namespace Dumbogram.Database.KeysetPagination.Dto.Strategies.Exceptions;

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