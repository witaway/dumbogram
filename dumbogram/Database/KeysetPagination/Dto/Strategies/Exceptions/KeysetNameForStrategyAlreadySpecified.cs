namespace Dumbogram.Database.KeysetPagination.Dto.Strategies.Exceptions;

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