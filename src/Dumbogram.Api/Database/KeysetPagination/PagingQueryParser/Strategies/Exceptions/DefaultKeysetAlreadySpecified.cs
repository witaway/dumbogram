namespace Dumbogram.Api.Database.KeysetPagination.PagingQueryParser.Strategies.Exceptions;

public class DefaultKeysetAlreadySpecified : ApplicationException
{
    public DefaultKeysetAlreadySpecified()
    {
        
    }

    public DefaultKeysetAlreadySpecified(string message)
        : base(message)
    {
        
    }
}