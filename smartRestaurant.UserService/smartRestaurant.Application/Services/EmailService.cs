
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using smartRestaurant.Application.ServiceContracts;
using MailKit.Net.Smtp;
using smartRestaurant.Application.EmailSettings;


namespace smartRestaurant.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSetting _settings;

        public EmailService(IOptions<EmailSetting> options)
        {
            _settings = options.Value;
        }

        /// <summary>
        /// Metodo de registro para confirmar o email na criacao da conta, envia um email com o link de confirmacao
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="confirmationLink"></param>
        /// <returns></returns>
        public async Task SendConfirmationEmailAsync(string email, string name, string confirmationLink)
        {
            var subject = "Confirmação de Conta";
            var body = BuildConfirmationEmailBody(name, confirmationLink);

            await SendEmailAsync(email, subject, body);


        }

        /// <summary>
        /// Envia um email com o link para redefinição da password.
        /// </summary>
        public async Task SendResetPasswordEmailAsync(string email, string name, string resetLink)
        {
            var subject = "Redefinição da Password";
            var body = BuildResetPasswordEmailBody(name, resetLink);
            await SendEmailAsync(email, subject, body);
        }
        private async Task SendEmailAsync(string email, string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                    _settings.SenderName,
                    _settings.SenderEmail));

            message.To.Add(
                MailboxAddress.Parse(email));

            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var smtp = new SmtpClient();

            var socketOptions = _settings.EnableSsl
                ? SecureSocketOptions.StartTls
                : SecureSocketOptions.None;

            await smtp.ConnectAsync(
                _settings.Host,
                _settings.Port,
                socketOptions);

            await smtp.AuthenticateAsync(
                _settings.Username,
                _settings.Password);

            await smtp.SendAsync(message);
            // colocar exceccoes aqui
            await smtp.DisconnectAsync(true);
        }

        private string BuildConfirmationEmailBody(string name, string confirmationLink)
        {
            return $"""
                    <h2>Bem-vindo a Keni</h2>

                    <p>Olá <strong>{name}</strong>,</p>

                    <p>Obrigado por criar uma conta.</p>

                    <p>Clique no link abaixo para confirmar o seu email:</p>

                    <p>
                    <a href="{confirmationLink}">
                    Confirmar Conta
                    </a>
                    </p>

                    <p>Se não criou esta conta, ignore este email.</p>

                    <p>Equipa Keni</p>
                    """;

        }

        private string BuildResetPasswordEmailBody(string name, string resetLink)
        {
            return $"""
                    <h2>Bem-vindo a Keni</h2>

                    <p>Olá <strong>{name}</strong>,</p>

                    <p>Recebemos um pedido para redefinir a sua password.</p>

                    <p>Clique no link abaixo para redefinir a sua password:</p>

                    <p>
                    <a href="{resetLink}">
                    Redefinir Password
                    </a>
                    </p>

                    <p>Se não solicitou a redefinição da password, ignore este email.</p>

                    <p>Equipa Keni</p>
                    """;
        }




    }
}
