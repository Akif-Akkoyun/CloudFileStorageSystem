using App.Application.Dtos.MailDtos;
using Ardalis.Result;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using App.Application.Interfaces.Auth;

namespace App.Persistence.Auth.Services.MailServices
{
    internal class MailService : IMailService
    {
        private readonly IConfiguration _config;

        public MailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<Result> SendMailAsync(MailSendDto mailSendDto)
        {
            var host = _config["Email:Smtp:Host"];
            if (string.IsNullOrWhiteSpace(host))
            {
                return Result.Error("Host value is not valid in configuration.");
            }
            if (!int.TryParse(_config["Email:Smtp:Port"], out var port))
            {
                return Result.Error("Port value is not valid in configuration.");
            }
            var username = _config["Email:Smtp:Username"];
            if (string.IsNullOrWhiteSpace(username))
            {
                return Result.Error("From value is not valid in configuration.");
            }
            var password = _config["Email:Smtp:Password"];
            if (string.IsNullOrWhiteSpace(password))
            {
                return Result.Error("Password value is not valid in configuration.");
            }
            if (!bool.TryParse(_config["Email:Smtp:EnableSsl"], out var enableSsl))
            {
                return Result.Error("EnableSsl value is not valid in configuration.");
            }
            using SmtpClient client = new(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSsl
            };
            var email = new MailMessage
            {
                From = new MailAddress(username),
                Subject = mailSendDto.Subject,
                Body = mailSendDto.Body,
                IsBodyHtml = mailSendDto.IsHtml,
            };
            foreach (var to in mailSendDto.To)
            {
                email.To.Add(to);
            }
            if (mailSendDto.Cc != null)
            {
                foreach (var cc in mailSendDto.Cc)
                {
                    email.CC.Add(cc);
                }
            }
            if (mailSendDto.Bcc != null)
            {
                foreach (var bcc in mailSendDto.Bcc)
                {
                    email.Bcc.Add(bcc);
                }
            }
            await client.SendMailAsync(email);
            return Result.Success();
        }
    }
}