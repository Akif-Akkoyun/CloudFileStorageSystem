using App.Application.Dtos.AuthDtos;
using App.Application.Features.Commads.AuthCommands;
using App.Application.Interfaces;
using App.Domain.Entities;
using Ardalis.Result;
using FluentValidation;
using MediatR;

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
                SurName = dto.UserSurName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash),
                RoleId = 2
            };
            await _repository.CreatAsync(user);
            return user;
        }
    }
}
