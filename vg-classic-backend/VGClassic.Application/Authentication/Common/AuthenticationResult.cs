using System.Collections.Generic;
using System.Linq;

namespace VGClassic.Application.Authentication.Common;

public class AuthenticationResult
{
    public bool IsSuccess { get; set; }
    public TokenResponse? Token { get; set; }
    public string? UserId { get; set; }
    public List<string> Errors { get; set; } = new();

    public static AuthenticationResult Success(TokenResponse token, string userId) =>
        new() { IsSuccess = true, Token = token, UserId = userId };

    public static AuthenticationResult Failure(string error) =>
        new() { IsSuccess = false, Errors = new List<string> { error } };

    public static AuthenticationResult Failure(IEnumerable<string> errors) =>
        new() { IsSuccess = false, Errors = errors.ToList() };
}

public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
}
