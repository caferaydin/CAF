namespace CAF.Application.Models.Authentication.Request;

public class ResetTokenRequest
{
    public string ResetToken { get; set; }
    public string UserId { get; set; }
}
