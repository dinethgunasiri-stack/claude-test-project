using MediatR;
using VGClassic.Application.Authentication.Common;

namespace VGClassic.Application.Authentication.Commands.Login;

public record LoginCommand : IRequest<AuthenticationResult>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
