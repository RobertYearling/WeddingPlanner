using System;
using System.ComponentModel.DataAnnotations;

namespace PlannerTwo.Validations
{
    public class PastDateAttribute : ValidationAttribute //PastDate Plus Attribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(DateTime.Now > (DateTime)value)
            {
                return new ValidationResult("You can not live in the past");
            }
            return ValidationResult.Success;
        }
    }
}