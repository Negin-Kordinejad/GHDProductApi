using FluentValidation;
using GHDProductApi.Core.Responses;
using MediatR;

namespace GHDProductApi.Core.Common.Behaviours
{
    public class RequestValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(validator => validator.Validate(context))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationFailure => validationFailure != null)
                .ToList();


            if (failures.Count == 0)
            {
                return await next();
            }

            var notFound = failures.Find(x => x.PropertyName.Equals("NotFoundFailure"));
            if (notFound != null)
            {
                throw new NotFoundException(notFound.ErrorMessage);
            }

            throw new ValidationException(failures);
        }
    }
}
