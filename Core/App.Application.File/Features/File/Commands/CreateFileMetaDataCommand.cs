using App.Application.Dtos.FileDtos;
using MediatR;

namespace App.Application.Features.File.Commands
{
    public class CreateFileMetaDataCommand : IRequest<int>
    {
        public FileCreateDto File { get; set; }

        public CreateFileMetaDataCommand(FileCreateDto file)
        {
            File = file;
        }
    }
}