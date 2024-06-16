namespace Infrastructure.Configs;
public class EmailCreds
{
    public string EmailHost { get; set; } = string.Empty;

    public string EmailUserName { get; set; } = string.Empty;

    public string EmailPassword { get; set; } = string.Empty;

    public string ResetPasswordUrl { get; set; } = string.Empty;
}
