using App.Application.File.Features.File.Queries;
using App.Application.File.Features.File.Results;
using App.Application.Interfaces.File;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.File.Features.File.Handlers
{
    public class GetUsersWithFileSharedQueryHandler : IRequestHandler<GetUsersWithFileSharedQuery, List<GetUsersWithFileSharedQueryResult>>
    {
        private readonly IFileShareRepository _fileShareRepository;
        public GetUsersWithFileSharedQueryHandler(IFileShareRepository fileShareRepository)
        {
            _fileShareRepository = fileShareRepository;
        }
        public async Task<List<GetUsersWithFileSharedQueryResult>> Handle(GetUsersWithFileSharedQuery request, CancellationToken cancellationToken)
        {
            var users = await _fileShareRepository.GetUsersWithFileSharedAsync(request.Id);
            return users.Select(x => new GetUsersWithFileSharedQueryResult
            {
                UserId = x.UserId,
                Permission = x.Permission
            }).ToList();
        }
    }
}
