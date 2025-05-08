using App.Application.Dtos.MailDtos;
using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IMailService
    {
        Task<Result> SendMailAsync(MailSendDto mailSendDto);
    }
}
