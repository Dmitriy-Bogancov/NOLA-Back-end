using Microsoft.Extensions.Options;
using MimeKit;
using NOLA_API.Infrastructure.Messages;
using NOLA_API.Interfaces;
using System.Net.Mail;

namespace NOLA_API.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(IOptions<EmailConfiguration> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendAsync(string email, Message message)
        {
            var emailMessage = CreateEmailMessage(email, message);

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(_emailConfig.Host, _emailConfig.Port, false);
                await client.AuthenticateAsync(_emailConfig.Email, _emailConfig.Password);

                try
                {
                    await client.SendAsync(emailMessage);
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    for (int i = 0; i < ex.InnerExceptions.Length; i++)
                    {
                        SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                        if (status == SmtpStatusCode.MailboxBusy || status == SmtpStatusCode.MailboxUnavailable)
                        {
                            await client.SendAsync(emailMessage);
                        }
                    }
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        private MimeMessage CreateEmailMessage(string email, Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("NOLA Administration", _emailConfig.Email));
            emailMessage.To.Add(new MailboxAddress(string.Empty, email));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            return emailMessage;
        }
    }
}
