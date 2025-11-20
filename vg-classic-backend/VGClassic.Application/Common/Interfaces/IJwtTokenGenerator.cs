using System.Collections.Generic;

namespace VGClassic.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userId, string email, List<string> roles);
    string GenerateRefreshToken();
}
