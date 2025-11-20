using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VGClassic.Application.Authentication.Common;
using VGClassic.Application.Common.Interfaces;
using VGClassic.Domain.Enums;

namespace VGClassic.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return _identityService.RegisterAsync(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            UserRole.Customer);
    }
}
