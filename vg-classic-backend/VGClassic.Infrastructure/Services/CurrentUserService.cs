using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using VGClassic.Application.Common.Interfaces;

namespace VGClassic.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public bool IsAdmin => _httpContextAccessor.HttpContext?.User?.IsInRole("Admin") == true ||
                           _httpContextAccessor.HttpContext?.User?.IsInRole("SuperAdmin") == true;
}
