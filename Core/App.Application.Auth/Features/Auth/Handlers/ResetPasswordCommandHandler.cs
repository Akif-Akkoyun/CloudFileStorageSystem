using App.Application.Features.Auth.Commands;
using App.Application.Interfaces.Auth;
using Ardalis.Result;
using MediatR;
namespace App.Application.Features.Auth.Handlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.PasswordHash != request.PasswordRepeat)
            {
                return Result.Invalid(new ValidationError("Passwords do not match"));
            }
            var user = await _userRepository
                .GetByFilterAsync(x =>
                x.RefreshPasswordToken == request.Token && x.Email == request.Email);
            if (user is null)
            {
                return Result.NotFound();
            }
            var cryptoPass = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
            user.PasswordHash = cryptoPass;
            user.RefreshPasswordToken = null;
            await _userRepository.UpdateAsync(user);
            return Result.Success();
        }
    }
}