using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Application.DTO;
using Domain.Entities.Support;
using FluentValidation;

namespace Application.Validators
{
    public class TicketEditDtoValidator : AbstractValidator<TicketEditDTO>
    {
        public TicketEditDtoValidator()
        {
            RuleSet("all", () =>
            {
                RuleFor(x => x.TicketId).Must(it => it > 0).WithMessage("id должно быть больше 0");
                
                RuleFor(x => x.CheckingUserId).Must(it => it != null)
                    .WithMessage("Админ, который редактирует тикет, должен быть означен");

                // todo: заменить рефлекшен на более быстрый способ
                RuleFor(x => x.State).Must(it => it > 0 || Enum.IsDefined(typeof(State), it)).WithMessage("Неизвестное состояние");
            });
        }
    }
}
