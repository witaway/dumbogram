namespace Dumbogram.Api.Database.KeysetPagination.PagingQueryParser.Strategies.Exceptions;

public class DefaultKeysetWasNotSpecified : ApplicationException
{
    public DefaultKeysetWasNotSpecified()
    {
        
    }

    public DefaultKeysetWasNotSpecified(string message)
        : base(message)
    {
        
    }
}