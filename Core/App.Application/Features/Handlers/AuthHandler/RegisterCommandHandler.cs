using App.Application.Features.Commads.AuthCommands;
using App.Application.Interfaces;
using App.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Features.Handlers.AuthHandler
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserEntity>
    {
        private readonly IRepository<UserEntity> _repository;

        public RegisterCommandHandler(IRepository<UserEntity> repository)
        {
            _repository = repository;
        }

        public async Task<UserEntity> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RegisterDto;

            var user = new UserEntity
            {
                Name = dto.UserName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash),
                RoleId = 2
            };
            await _repository.CreatAsync(user);
            return user;
        }
    }
}
