namespace CAF.Application.Abstractions.Services.Session
{
    public interface ICurrentUserSession
    {
        bool IsAuthenticated { get; }
        string? UserId { get; }
        string? UserName { get; }
        string? Email { get; }
        string? IpAddress { get; }
        HashSet<string> Permissions { get; set; } 
        List<string> RoleIds { get; } 
    }
}
