using App.Application.File.Dtos;
using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.File.Features.File.Commands
{
    public class ShareFileCommand : IRequest<Result>
    {
        public ShareFileDto Dto { get; }

        public ShareFileCommand(ShareFileDto dto)
        {
            Dto = dto;
        }
    }
}
