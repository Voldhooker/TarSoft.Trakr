namespace TarSoft.Mediator
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    public class SimpleMediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommandDispatcher _commandDispatcher;

        public SimpleMediator(IServiceProvider serviceProvider, ICommandDispatcher commandDispatcher)
        {
            _serviceProvider = serviceProvider;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var requestType = request.GetType();
            var isCommand = typeof(ICustomCommand).IsAssignableFrom(requestType);

            if (isCommand)
            {
                //Ugh I do not like this. Not sure how to make it better
                var responseType = typeof(TResponse);
                var dispatchMethod = typeof(CommandDispatcher).GetMethod("Dispatch").MakeGenericMethod(requestType, responseType);
                return await (Task<TResponse>)dispatchMethod.Invoke(_commandDispatcher, new object[] { (dynamic)request, cancellationToken });
            }
            else
            {
                // Resolve and call the query handler directly. TODO: Modify for pipeline behaviors 
                var handlerType = typeof(IQueryHandler<,>).MakeGenericType(requestType, typeof(TResponse));
                dynamic handler = _serviceProvider.GetRequiredService(handlerType);

                if (handler == null)
                {
                    throw new InvalidOperationException($"Handler for {requestType.Name} not found.");
                }

                return await handler.Handle((dynamic)request, cancellationToken);
            }
        }
    }




}
