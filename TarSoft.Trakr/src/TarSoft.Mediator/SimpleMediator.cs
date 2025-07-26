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
        private readonly IQueryDispatcher _queryDispatcher;

        public SimpleMediator(IServiceProvider serviceProvider, ICommandDispatcher commandDispatcher)
        {
            _serviceProvider = serviceProvider;
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = new QueryDispatcher(serviceProvider);
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var requestType = request.GetType();
            var isCommand = typeof(ICustomCommand).IsAssignableFrom(requestType);

            if (isCommand)
            {
                // Use dynamic dispatch to call the strongly-typed command dispatcher
                // This is cleaner than reflection and still benefits from pipeline behaviors
                return await ((dynamic)_commandDispatcher).Dispatch((dynamic)request, cancellationToken);
            }
            else
            {
                // Use dynamic dispatch for queries as well for consistency
                return await ((dynamic)_queryDispatcher).Dispatch((dynamic)request, cancellationToken);
            }
        }
    }




}
