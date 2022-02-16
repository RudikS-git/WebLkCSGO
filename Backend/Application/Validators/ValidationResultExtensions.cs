using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation.Results;

namespace Application.Validators
{
    public static class ValidationResultExtensions
    {
        public static void GetExceptionFromNotValide(this ValidationResult result)
        {
            if (!result.IsValid)
            {
                new ValidationResult(result.Errors);
            }
        }
    }
}
