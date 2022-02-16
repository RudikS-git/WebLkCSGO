using System;
using System.Collections.Generic;
using System.Text;
using Application.DTO;
using FluentValidation;

namespace Application.Validators
{
    public class ServerDtoValidator : AbstractValidator<ServerDTO>
    {
        public ServerDtoValidator()
        {
            RuleSet("all", () =>
            {
                RuleFor(x => x.Ip)
                    .Must(it => string.IsNullOrEmpty(it)).WithMessage("Невалидный IP");

                RuleFor(x => x.Port)
                    .Must(it => !IsValidPort(it))
                    .WithMessage("Невалидный port");
            });
        }

        public bool IsValidPort(int port)
        {
            return port >= 0 && port <= 65535;
        }
    }
}
