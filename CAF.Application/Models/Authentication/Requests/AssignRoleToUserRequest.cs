namespace CAF.Application.Models.Authentication.Requests;

public class AssignRoleToUserRequest
{
    public string UserId { get; set; }

    public string[] Roles { get; set; }
}
