﻿namespace Dumbogram.Infrasctructure.Errors;

public class AuthenticationTokenIncorrectError : ApplicationError
{
    public AuthenticationTokenIncorrectError()
        : base(nameof(AuthenticationTokenIncorrectError))
    {
    }
}