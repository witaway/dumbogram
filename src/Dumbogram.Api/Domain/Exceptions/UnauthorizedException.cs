﻿using ApplicationException = Dumbogram.Api.Common.Exceptions.ApplicationException;

namespace Dumbogram.Api.Domain.Exceptions;

public class UnauthorizedException : ApplicationException
{
    public UnauthorizedException()
    {
    }

    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string message, Exception inner) : base(message, inner)
    {
    }
}