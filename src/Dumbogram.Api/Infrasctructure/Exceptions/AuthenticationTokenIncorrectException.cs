﻿namespace Dumbogram.Api.Infrasctructure.Exceptions;

public class AuthenticationTokenIncorrectException : ApplicationException
{
    public AuthenticationTokenIncorrectException(string message)
        : base(message)
    {
    }

    public AuthenticationTokenIncorrectException(string message, Exception inner)
        : base(message, inner)
    {
    }
}