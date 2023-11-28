namespace Dumbogram.Infrasctructure.Errors;

public class ApplicationInternalError : ApplicationError
{
    public ApplicationInternalError(string errorCode)
        : base(errorCode)
    {
    }
}