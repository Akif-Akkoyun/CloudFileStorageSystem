using App.Application.Features.Auth.Commands;
using App.Application.Interfaces.Auth;
using App.Domain.Entities;
using MediatR;

namespace App.Application.Features.Auth.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserEntity>
    {
        private readonly IUserRepository _repository;
        public RegisterCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserEntity> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RegisterDto;
            
            var user = new UserEntity
            {
                Name = dto.UserName,
                SurName = dto.UserSurName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash),
                RoleId = 2
            };
            await _repository.CreateAsync(user);
            return user;
        }
    }
}
