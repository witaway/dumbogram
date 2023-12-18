namespace Dumbogram.Database.KeysetPagination.Dto.Strategies.Exceptions;

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