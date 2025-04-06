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
    public class PageValidator : AbstractValidator<Query>
    {
        private readonly int[] DostepneWilekosci = new int[] {3,5,10,15};
        public PageValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!DostepneWilekosci.Contains(value))
                {
                    context.AddFailure("Wielkość strony", $"Wielkość strony musi wynosić {string.Join(" lub ",DostepneWilekosci)}");
                }
            });
        }
    }
}

