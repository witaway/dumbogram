namespace Dumbogram.Database.KeysetPagination.Dto.Strategies.Exceptions;

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