using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTemplate.Application
{
    public static class ValidationFailureExtensions
    {
        public static ModelValidationResult ToModelValidationResult(this IList<ValidationFailure> validationFailures, string message)
        {
            var validationResult = new ModelValidationResult
            {
                Message = message,
                Errors = new List<ValidationError>()
            };

            foreach (var failure in validationFailures)
            {
                validationResult.Errors.Add(new ValidationError
                {
                    ErrorMessage = failure.ErrorMessage,
                    PropertyName = failure.PropertyName
                });
            }

            return validationResult;
        }
    }
}
