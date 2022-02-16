using System;
using System.Collections.Generic;
using System.Text;
using Application.DTO;
using Application.DTO.Input;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ValidationRegistration
    {
        public static void AddValidationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IValidator<PurchaseDto>, PurchaseDtoValidator>();
            services.AddSingleton<IValidator<ServerDTO>, ServerDtoValidator>();
            services.AddSingleton<IValidator<TicketEditDTO>, TicketEditDtoValidator>();
        }
    }
}
