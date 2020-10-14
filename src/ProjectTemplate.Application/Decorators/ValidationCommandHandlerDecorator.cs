using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ProjectTemplate.Application.Abstractions.Commands;

namespace ProjectTemplate.Application.Decorators
{
    public class ValidationCommandHandlerDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationCommandHandlerDecorator(
            IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var errors = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (errors.Any())
            {
               throw new ValidationException(errors.ToModelValidationResult($"validation failed for {typeof(TRequest).Name}"));
            }

            return next();
        }
    }
}