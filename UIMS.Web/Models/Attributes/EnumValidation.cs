using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.Models.Attributes
{
    public class EnumValidation:ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Enum.IsDefined(validationContext.ObjectType, value))
                return ValidationResult.Success;

            return new ValidationResult($"{validationContext.MemberName} Is Invalid");
        }
    }
}
