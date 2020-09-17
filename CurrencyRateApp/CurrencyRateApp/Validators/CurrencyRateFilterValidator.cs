using CurrencyRateApp.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyRateApp.Validators
{
    public class CurrencyRateFilterValidator : AbstractValidator<CurrencyRateFilter>
    {
        public CurrencyRateFilterValidator()
        {
            RuleFor(c => c.CurrencyCodes)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Currency codes can not be null")
                .Must((c, CurrencyCodes) => { return ValidateKeyAndValues(c.CurrencyCodes); })
                    .WithMessage("Currency codes can not contains empty key or value")
                .Must((c, CurrencyCodes) => { return ValidateKeyAndValuesLength(c.CurrencyCodes); })
                    .WithMessage("Currency codes has to have 3 characters length");

            RuleFor(c => c.StartDate)
                .NotEmpty().WithMessage("Start date can not be null")
                .Must((c, StartDate) => { return ValidateDateOrder(c.StartDate, c.EndDate); })
                    .WithMessage("Start date has to be earlier or equal to end date");

            RuleFor(c => c.EndDate)
                .NotEmpty().WithMessage("End date can not be null")
                .Must((c, EndDate) => { return ValidateDateValue(c.EndDate); });
        }

        private bool ValidateKeyAndValuesLength(IDictionary<string, string> currencyCodes)
        {
            var result = true;
            if (currencyCodes.Keys.Any(ck => ck.Length != 3))
            {
                result = false;
            }
            else if(currencyCodes.Values.Any(cv => cv.Length != 3))
            {
                result = false;
            }

            return result;
        }

        private bool ValidateKeyAndValues(IDictionary<string, string> currencyCodes)
        {
            var result = true;
            if (currencyCodes.Count == 0)
            {
                result = false;
            }
            else if (currencyCodes.ContainsKey(string.Empty))
            {
                result = false;
            }
            else if (currencyCodes.Values.Any(cv => string.IsNullOrEmpty(cv)))
            {
                result = false;
            }

            return result;
        }

        private bool ValidateDateOrder(DateTime startDate, DateTime endDate)
             => DateTime.Compare(startDate, endDate) <= 0;

        private bool ValidateDateValue(DateTime endDate)
        {
            if (DateTime.Compare(endDate, DateTime.UtcNow) > 0)
            {
                throw new Exception("Date can not be from the future");
            }

            return true;
        }
    }
}
