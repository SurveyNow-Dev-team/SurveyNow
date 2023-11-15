using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CustomizeAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class MinAgeAttribute: ValidationAttribute
    {
        private int _minAge;

        public MinAgeAttribute(int minAge)
        {
            _minAge = minAge;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            DateTime? date = (DateTime?)value;
            if (date.HasValue)
            {
                var difference = DateTime.Now.AddYears(-_minAge);
                if(difference < date.Value)
                {
                    return new ValidationResult($"Age is smaller than {_minAge}");
                }
            }
            return ValidationResult.Success;
        }
    }
}
