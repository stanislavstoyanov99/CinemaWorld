namespace CinemaWorld.Common.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    [AttributeUsage(AttributeTargets.Property)]
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            this.maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > this.maxFileSize)
                {
                    return new ValidationResult(this.GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return string.Format(GlobalConstants.MaxFileSizeErrorMessage, this.maxFileSize / 1024 / 1024);
        }
    }
}
