using App.Application.Dtos.MailDtos;
using App.Application.Features.Auth.Commands;
using App.Application.Interfaces.Auth;
using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace App.Application.Features.Auth.Handlers
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        public ForgotPasswordCommandHandler(IUserRepository userRepository, IMailService mailService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mailService = mailService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByFilterAsync(u => u.Email == request.Email);

            if (user == null)
                return Result.NotFound();

            user.RefreshPasswordToken = Guid.NewGuid().ToString("n");
            var baseUrl = _configuration["Frontend:BaseUrl"];
            var resetLink = $"{baseUrl}/renew-password/{user.RefreshPasswordToken}";
            var mailSend = new MailSendDto
            {
                To = [user.Email],
                Subject = "Şifre Sıfırlama",
                Body = $"<p style='display:none;'>Bu mail, şifre sıfırlama talebiniz içindir.</p>" + $"Merhaba {user.Name.ToUpper()},<br>Şifrenizi sıfırlamak için <a href='{resetLink}'>tıklayınız</a></br>",
                IsHtml = true
            };
            var mailResult = await _mailService.SendMailAsync(mailSend);
            if (!mailResult.IsSuccess)
                return Result.Invalid(new ValidationError("Mail could not be sent"));
            await _userRepository.UpdateAsync(user);
            return Result.Success();
        }
    }
}