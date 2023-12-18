namespace Dumbogram.Database.KeysetPagination.Dto.Strategies.Exceptions;

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