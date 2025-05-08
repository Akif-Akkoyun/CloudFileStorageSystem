using App.Application.Features.Commads.AuthCommands;
using App.Application.Interfaces;
using App.Domain.Entities;
using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Features.Handlers.AuthHandler
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly IRepository<UserEntity> _userRepository;

        public ResetPasswordCommandHandler(IRepository<UserEntity> userRepository)
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
            if (user == null)
            {
                return Result.NotFound();
            }
            user.PasswordHash = request.PasswordHash;
            user.RefreshPasswordToken = null;
            await _userRepository.UpdateAsync(user);
            return Result.Success();
        }
    }
}
