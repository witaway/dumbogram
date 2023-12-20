namespace Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.PagingQueryParser.Strategies.Exceptions;

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