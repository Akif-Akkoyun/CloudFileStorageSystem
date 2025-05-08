using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Dtos.MailDtos
{
    public class MailSendDto
    {
        public string[] To { get; set; } = null!;
        public string[]? Cc { get; set; }
        public string[]? Bcc { get; set; }
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public bool IsHtml { get; set; }
    }
    public class MailSendDTOValidator : AbstractValidator<MailSendDto>
    {
        public MailSendDTOValidator()
        {
            RuleFor(x => x.To).NotEmpty().ForEach(x => x.EmailAddress());
            RuleFor(x => x.Cc).ForEach(x => x.EmailAddress());
            RuleFor(x => x.Bcc).ForEach(x => x.EmailAddress());
            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.Body).NotEmpty();
        }
    }
}
