namespace Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.PagingQueryParser.Strategies.Exceptions;

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