namespace Dumbogram.Api.Database.KeysetPagination.PagingQueryParser.Strategies.Exceptions;

public class FieldNameForStrategyAlreadySpecified : ApplicationException
{
    public FieldNameForStrategyAlreadySpecified()
    {
        
    }

    public FieldNameForStrategyAlreadySpecified(string message)
        : base(message)
    {
        
    }
}