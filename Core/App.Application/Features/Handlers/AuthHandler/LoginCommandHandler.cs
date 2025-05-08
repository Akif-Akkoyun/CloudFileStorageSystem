using App.Application.Dtos.AuthDtos;
using App.Application.Features.Commads.AuthCommands;
using App.Application.Interfaces;
using App.Domain.Entities;
using MediatR;
using Hasher = BCrypt.Net.BCrypt;

namespace App.Application.Features.Handlers.AuthHandler
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthTokenDto>
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IAuthTokenService _authTokenService;
        public LoginCommandHandler(IRepository<UserEntity> userRepository, IAuthTokenService authTokenService)
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
            if (user.RoleId != 1)
            {
                throw new UnauthorizedAccessException("Unauthorized Access");
            }
            var token = _authTokenService.GenerateToken(user);
            return new AuthTokenDto { Token = token };
        }
    }
}