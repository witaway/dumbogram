namespace Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.PagingQueryParser.Strategies.Exceptions;

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