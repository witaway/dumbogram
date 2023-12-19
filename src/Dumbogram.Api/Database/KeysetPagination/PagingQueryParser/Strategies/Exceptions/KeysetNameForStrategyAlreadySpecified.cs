namespace Dumbogram.Api.Database.KeysetPagination.PagingQueryParser.Strategies.Exceptions;

public class KeysetNameForStrategyAlreadySpecified : ApplicationException
{
    public KeysetNameForStrategyAlreadySpecified()
        : base()
    {
        
    }

    public KeysetNameForStrategyAlreadySpecified(string message)
        : base(message)
    {
        
    }
}