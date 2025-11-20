using MediatR;
using VGClassic.Application.Authentication.Common;

namespace VGClassic.Application.Authentication.Commands.Register;

public record RegisterCommand : IRequest<AuthenticationResult>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}
