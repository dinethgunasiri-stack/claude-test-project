using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VGClassic.Application.Authentication.Common;
using VGClassic.Application.Common.Interfaces;

namespace VGClassic.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return _identityService.LoginAsync(request.Email, request.Password);
    }
}
