﻿namespace Dumbogram.Api.Common.Errors;

public class ApplicationInternalError : ApplicationError
{
    public ApplicationInternalError(string errorCode)
        : base(errorCode)
    {
    }
}