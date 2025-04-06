using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace ApiP.Models.Valid
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(r => r.Password).NotEmpty();
            RuleFor(r => r.Password).MinimumLength(4);
            RuleFor(r => r.Password).Custom((value,context) =>{
                if(!value.Any(char.IsUpper) || !value.Any(char.IsDigit))
                {
                    context.AddFailure("Password", "Hasło musi zawierać minimum jedną dużą literę i liczbę");
                }
            });
        }
    }
}

