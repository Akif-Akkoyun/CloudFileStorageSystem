using App.Application.Dtos.FileDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Features.File.Commands
{
    public class UpdateFileMetaDataCommand : IRequest<bool>
    {
        public FileUpdateDto File { get; set; }

        public UpdateFileMetaDataCommand(FileUpdateDto file)
        {
            File = file;
        }
    }
}
