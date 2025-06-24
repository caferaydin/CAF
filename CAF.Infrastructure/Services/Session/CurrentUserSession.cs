using System.Security.Claims;
using CAF.Application.Abstractions.Services.Session;
using Microsoft.AspNetCore.Http;

namespace CAF.Infrastructure.Services.Session;

public class CurrentUserSession : ICurrentUserSession
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserSession(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public string? UserId =>
        User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public string? UserName =>
        User?.FindFirst(ClaimTypes.Name)?.Value;

    public string? Email =>
        User?.FindFirst(ClaimTypes.Email)?.Value;

    public string? IpAddress =>
        _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

    public List<string> RoleIds =>
        _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role)
            .Select(r => r.Value).ToList() ?? new List<string>();

    public HashSet<string> Permissions { get; set; } = new();
}
