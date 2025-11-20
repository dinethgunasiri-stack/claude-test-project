using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VGClassic.Application.Authentication.Common;
using VGClassic.Application.Common.Interfaces;
using VGClassic.Domain.Enums;

namespace VGClassic.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthenticationResult> RegisterAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        UserRole role = UserRole.Customer)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return AuthenticationResult.Failure("User with this email already exists");
        }

        var user = new ApplicationUser
        {
            Email = email,
            UserName = email,
            FirstName = firstName,
            LastName = lastName,
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return AuthenticationResult.Failure(result.Errors.Select(e => e.Description));
        }

        await _userManager.AddToRoleAsync(user, role.ToString());

        var token = await GenerateTokenAsync(user);
        return AuthenticationResult.Success(token, user.Id);
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !user.IsActive)
        {
            return AuthenticationResult.Failure("Invalid credentials");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            return AuthenticationResult.Failure("Invalid credentials");
        }

        user.LastLoginDate = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var token = await GenerateTokenAsync(user);
        return AuthenticationResult.Success(token, user.Id);
    }

    public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }

        return await GenerateTokenAsync(user);
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    private async Task<TokenResponse> GenerateTokenAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtTokenGenerator.GenerateToken(user.Id, user.Email!, roles.ToList());
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = 3600
        };
    }
}
