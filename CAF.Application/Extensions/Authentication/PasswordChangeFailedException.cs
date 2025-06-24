namespace CAF.Application.Extensions.Authentication;

public class PasswordChangeFailedException : Exception
{
    public PasswordChangeFailedException() : base("Şifre güncellenirken bir sorun oluştu.") 
    {
    }

    public PasswordChangeFailedException(string? message) : base(message)
    {
    }

    public PasswordChangeFailedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}