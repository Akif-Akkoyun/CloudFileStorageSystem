using App.Application.Auth.Features.Auth.Queries;
using App.Application.Auth.Features.Auth.Results;
using App.Application.Interfaces.Auth;
using MediatR;
namespace App.Application.Auth.Features.Auth.Handlers
{
    public class GetUserByIdQueryHandler :IRequestHandler<GetUserByIdQuery, GetUserByIdQueryResult>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserByIdQueryResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetByIdAsync(request.Id);
            return new GetUserByIdQueryResult
            {
                Id = users.Id,
                Name = users.Name,
                SurName = users.SurName,
                Email = users.Email,
                RoleId = users.RoleId,
                PasswordHash = users.PasswordHash,
                RefreshPasswordToken = users.RefreshPasswordToken,
            };
        }
    }
}
