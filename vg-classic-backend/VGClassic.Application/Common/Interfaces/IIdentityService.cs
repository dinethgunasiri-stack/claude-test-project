using System.Threading.Tasks;
using VGClassic.Application.Authentication.Common;
using VGClassic.Domain.Enums;

namespace VGClassic.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<AuthenticationResult> RegisterAsync(string email, string password, string firstName, string lastName, UserRole role = UserRole.Customer);
    Task<AuthenticationResult> LoginAsync(string email, string password);
    Task<TokenResponse?> RefreshTokenAsync(string refreshToken);
    Task<bool> UserExistsAsync(string email);
}
