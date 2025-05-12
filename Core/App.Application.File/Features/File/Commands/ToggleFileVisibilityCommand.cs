using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.File.Features.File.Commands
{
    public class ToggleFileVisibilityCommand : IRequest<Result>
    {
        public int FileId { get; set; }

        public ToggleFileVisibilityCommand(int fileId)
        {
            FileId = fileId;
        }
    }

}
