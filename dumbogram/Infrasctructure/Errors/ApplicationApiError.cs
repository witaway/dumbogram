using System.Net;

namespace Dumbogram.Infrasctructure.Errors;

public class ApplicationApiError : ApplicationError
{
    public ApplicationApiError(string errorCode, HttpStatusCode statusCode)
        : base(errorCode)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; private set; }
}