using Microsoft.AspNetCore.Identity;

namespace CAF.Domain.Entities.Authentication;
public class AppRole : IdentityRole<string>
{
    public string? Description { get; set; }
}