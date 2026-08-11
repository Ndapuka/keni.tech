using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using smartRestaurant.Application.EmailSettings;


namespace smartRestaurant.Application.ServiceContracts;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(string email, string name, string confirmationLink);

    Task SendResetPasswordEmailAsync(string email, string name, string resetLink);


}

public class EmailService : IEmailService
{
    private readonly EmailSetting _settings;
    public EmailService(IOptions<EmailSetting> options)
    {
        _settings = options.Value;
    }
    public async Task SendConfirmationEmailAsync(string email, string name, string confirmationLink)
    {
        var message = new MimeMessage();
        message.From.Add(
            new MailboxAddress(
                _settings.SenderName,
                _settings.SenderEmail));
        message.To.Add(
            MailboxAddress.Parse(email));
        message.Subject = "Confirmação de Conta";
        message.Body = new TextPart("plain")
        {
            Text = $"Olá {name},\n\n" +
                   $"Por favor, clique no link abaixo para confirmar sua conta:\n" +
                   $"{confirmationLink}\n\n" +
                   "Se você não solicitou esta ação, por favor ignore este e-mail."
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(
            _settings.Host,
            _settings.Port,
            SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(
            _settings.Username,
            _settings.Password);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
    public async Task SendResetPasswordEmailAsync(string email, string name, string resetLink)
    {
        var message = new MimeMessage();
        message.From.Add(
            new MailboxAddress(
                _settings.SenderName,
                _settings.SenderEmail));
        message.To.Add(
            MailboxAddress.Parse(email));
        message.Subject = "Redefinição de Senha";
        message.Body = new TextPart("plain")
        {
            Text = $"Olá {name},\n\n" +
                   $"Por favor, clique no link abaixo para redefinir sua senha:\n" +
                   $"{resetLink}\n\n" +
                   "Se você não solicitou esta ação, por favor ignore este e-mail."
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(
            _settings.Host,
            _settings.Port,
            SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(
            _settings.Username,
            _settings.Password);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}

