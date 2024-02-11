using FluentResults;

namespace TarSoft.Mediator
{
    public class SimpleMediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public SimpleMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {

            //var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            Console.WriteLine($"Resolving handler type: {handlerType}");
            dynamic handler = _serviceProvider.GetService(handlerType);
            if (handler == null)
            {
                throw new InvalidOperationException($"Handler for {request.GetType().Name} not found.");
            }
            return await handler.Handle((dynamic)request, cancellationToken);
        }
    }

}
