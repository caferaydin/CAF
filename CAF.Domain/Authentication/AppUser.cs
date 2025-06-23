using CAF.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CAF.Domain.Authentication;

public class AppUser : IdentityUser
{
    public string? NameSurname { get; set; }
    public DateTime BirthDate { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public string? RefreshToken { get; set; }
    public int DepartmentId { get; set; } = Convert.ToInt32(DepartmentType.User);
    public DateTime? RefreshTokenEndDate { get; set; }
}