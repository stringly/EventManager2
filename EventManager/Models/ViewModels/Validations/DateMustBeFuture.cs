using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels.Validations
{
    public class DateMustBeFuture : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateValue = value as DateTime? ?? new DateTime();
            if (dateValue < DateTime.Now)
            {
                return new ValidationResult("Must be a future date.");
            }
            return ValidationResult.Success;
        }
    }
}
