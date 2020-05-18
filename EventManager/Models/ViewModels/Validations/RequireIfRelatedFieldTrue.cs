using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels.Validations
{
    public class RequireIfRelatedFieldTrue : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public RequireIfRelatedFieldTrue(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            // this is a change to handle an int? type on the attribute field
            // some VM fields will have int? type to prevent the validator from
            // flagging them as required if they are optional or conditionally optional.
            var currentValue = 0;
            if (value != null)
            {
                currentValue = (int)value;
            }

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (bool)property.GetValue(validationContext.ObjectInstance);

            if (comparisonValue == true && currentValue <= 0)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
