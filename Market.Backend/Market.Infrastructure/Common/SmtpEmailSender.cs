using Market.Application.Abstractions;
using Market.Application.Common.Email;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Infrastructure.Common
{
    public sealed class SmtpEmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;

        public SmtpEmailSender(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmail(string recieverEmail, string subject, string emailText, CancellationToken cancellationToken)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_settings.From),
                Subject = subject,
                Body =
                $"""
                {emailText}"



                Best luck, and game on!

                MaBO Games.
                """
            };


            message.To.Add(recieverEmail);

            var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = true
            };

            await client.SendMailAsync(message, cancellationToken);
        }

        public async Task SendPasswordRecoveryCode(string recieverEmail, string recoverCode, CancellationToken cancellationToken)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_settings.From),
                Subject = "Password reset code",
                Body =
                $"""
                Your password reset code is:

                {recoverCode}

                This code expires in 15 minutes.
                If you did not request a password reset, you can safely ignore this email.
                """
            };


            message.To.Add(recieverEmail);

            var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = true
            };

            await client.SendMailAsync(message, cancellationToken);


        }

    }
}

