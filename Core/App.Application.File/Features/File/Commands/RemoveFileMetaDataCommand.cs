using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Features.File.Commands
{
    public class RemoveFileMetaDataCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public RemoveFileMetaDataCommand(int id)
        {
            Id = id;
        }
    }
}
