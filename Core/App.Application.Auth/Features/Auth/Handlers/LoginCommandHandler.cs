using App.Application.Dtos.AuthDtos;
using App.Application.Features.Auth.Commands;
using App.Application.Interfaces.Auth;
using MediatR;
using Hasher = BCrypt.Net.BCrypt;

namespace App.Application.Features.Auth.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthTokenDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthTokenService _authTokenService;
        public LoginCommandHandler(IUserRepository userRepository, IAuthTokenService authTokenService)
        {
            _userRepository = userRepository;
            _authTokenService = authTokenService;
        }
        public async Task<AuthTokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByFilterAsync(u => u.Email == request.Email);
            if (user is null || !Hasher.Verify(request.PasswordHash, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Email or password is incorrect");
            }
            var token = _authTokenService.GenerateToken(user);
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token generation failed");
            }
            return new AuthTokenDto { Token = token };
        }
    }
}