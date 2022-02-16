using System;
using System.Collections.Generic;
using System.Text;
using Application.DTO.Input;
using Domain.Entities.Support;
using FluentValidation;
using SteamIDs_Engine;

namespace Application.Validators
{
    public class PurchaseDtoValidator : AbstractValidator<PurchaseDto>
    {
        public PurchaseDtoValidator()
        {
            RuleSet("all", () =>
            {
                RuleFor(x => x.SteamId)
                    .Must(it => string.IsNullOrEmpty(it))
                    .WithMessage("STEAM ID обязательный параметр!");

                RuleFor(x => x.SteamId)
                    .Must(it => !SteamIDConvert.IsValidSteamId(it))
                    .WithMessage("Невалидный STEAM ID");
            });

        }
    }
}
