using App.Application.File.Features.File.Commands;
using App.Application.Interfaces.File;
using App.Domain.Entities;
using App.Domain.Enums;
using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.File.Features.File.Handlers
{
    public class ShareFileCommandHandler : IRequestHandler<ShareFileCommand, Result>
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFileShareRepository _fileShareRepository;

        public ShareFileCommandHandler(
            IFileRepository fileRepository,
            IFileShareRepository fileShareRepository)
        {
            _fileRepository = fileRepository;
            _fileShareRepository = fileShareRepository;
        }

        public async Task<Result> Handle(ShareFileCommand request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetByIdAsync(request.Dto.FileId);
            if (file == null)
                return Result.NotFound();
            var shareEntities = request.Dto.SharedWithUserIds.Select(userId => new FileShareEntity
            {

                FileId = request.Dto.FileId,
                UserId = userId,
                Permission = request.Dto.Permission.ToString()
            });

            await _fileShareRepository.CreateRangeAsync(shareEntities); 

            return Result.Success();
        }
    }
}
