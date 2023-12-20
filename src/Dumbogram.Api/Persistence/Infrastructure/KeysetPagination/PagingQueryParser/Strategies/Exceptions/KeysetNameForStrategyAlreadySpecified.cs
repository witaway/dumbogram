namespace Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.PagingQueryParser.Strategies.Exceptions;

public class KeysetNameForStrategyAlreadySpecified : ApplicationException
{
    public KeysetNameForStrategyAlreadySpecified()
    {
    }

    public KeysetNameForStrategyAlreadySpecified(string message)
        : base(message)
    {
    }
}