using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using LiveLib.Application.Commom.ResultWrapper;
using MediatR;

namespace LiveLib.Application.Commom.Validation
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            var validationTasks = _validators.Select(v => v.ValidateAsync(context, cancellationToken));
            var validationResults = await Task.WhenAll(validationTasks);

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count == 0)
            {
                return await next();
            }

            return ConvertFailuresToErrorResponse(failures);
        }

        private TResponse ConvertFailuresToErrorResponse(List<ValidationFailure> failures)
        {
            var errorMessage = string.Join(Environment.NewLine, failures.Select(f => f.ErrorMessage));
            var responseType = typeof(TResponse);

            if (IsGenericResult(responseType))
            {
                var resultType = responseType.GetGenericArguments()[0];
                var method = typeof(Result<>)
                    .MakeGenericType(resultType)
                    .GetMethod("Validation", new[] { typeof(string) });

                if (method != null)
                {
                    return (TResponse)method.Invoke(null, new object[] { errorMessage });
                }
            }
            else if (responseType == typeof(Result))
            {
                return (TResponse)(object)Result.Validation(errorMessage);
            }

            throw new ValidationException(failures);
        }

        private static bool IsGenericResult(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>);
        }
    }
}