﻿namespace Dumbogram.Application.Auth.Dto;

public class SignInResponseDto
{
    public required string Token { get; set; }
    public required DateTime Expiration { get; set; }
}