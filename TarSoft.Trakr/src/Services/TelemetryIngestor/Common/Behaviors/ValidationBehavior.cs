using FluentResults;
using FluentValidation;
using TarSoft.Mediator;

namespace TelemetryIngestor.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken)
    {
        if (_validator == null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(x => $"{x.PropertyName}: {x.ErrorMessage}")
                .ToArray();

            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = typeof(TResponse).GetGenericArguments()[0];
                var failMethod = typeof(Result<>).MakeGenericType(resultType).GetMethod("Fail", new[] { typeof(string[]) });
                return (TResponse)failMethod!.Invoke(null, new object[] { errors })!;
            }

            if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Fail(errors);
            }
        }

        return await next();
    }
}
