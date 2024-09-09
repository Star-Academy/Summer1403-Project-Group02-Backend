namespace mohaymen_codestar_Team02.CleanArch1.Models;

public class CustomRegexPattern
{
    public const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$";
}