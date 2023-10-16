using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ParentBookingAPI.Helper;
using ParentBookingAPI.Model.DTO;
using ParentBookingAPI.Repository.Interfaces;
using ParentBookingAPI.Service;

namespace ParentBookingAPI.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly EmailSettings emailSettings;

        public EmailRepository(IOptions<EmailSettings> options)
        {
            this.emailSettings = options.Value;
        }
        public async Task SendEmailAsync(SendEmailRequestDto sendEmailRequestDto)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(sendEmailRequestDto.ToEmail));
            email.Subject = sendEmailRequestDto.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = sendEmailRequestDto.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(email);

            smtp.Disconnect(true);

        }
    }
}
