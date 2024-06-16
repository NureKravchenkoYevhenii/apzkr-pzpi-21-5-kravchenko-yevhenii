using BLL.Contracts;
using Infrastructure.Configs;
using System.Net;
using System.Net.Mail;

namespace BLL.Services;
public class EmailService : IEmailService
{
    private readonly Lazy<IUserService> _userService;
    private readonly EmailCreds _emailCreds;

    public EmailService(
        Lazy<IUserService> userService,
        EmailCreds emailCreds)
    {
        _userService = userService;
        _emailCreds = emailCreds;
    }

    public void SendResetPasswordEmail(string emailAddress)
    {
        var user = _userService.Value.GetByEmail(emailAddress);
        var resetPasswordToken = _userService.Value.GenerateResetPasswordToken(user.Id);

        var email = new MailMessage(
            new MailAddress(_emailCreds.EmailUserName),
            new MailAddress(emailAddress)
        );
        email.Subject = "Reset password";

        var resetPasswordUrl = string.Format(_emailCreds.ResetPasswordUrl, resetPasswordToken);

        email.IsBodyHtml = true;
        email.Body = $"<p>To reset password please follow the link</p><a href=\"{resetPasswordUrl}\">Reset password</a>";

        using var smtp = new SmtpClient(_emailCreds.EmailHost, 587);
        smtp.EnableSsl = true;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(_emailCreds.EmailUserName, _emailCreds.EmailPassword);
        smtp.Send(email);
    }
}
