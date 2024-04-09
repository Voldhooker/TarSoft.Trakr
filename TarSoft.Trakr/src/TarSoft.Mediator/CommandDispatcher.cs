using Microsoft.Extensions.DependencyInjection;

namespace TarSoft.Mediator
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;


        public CommandDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public async Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation)
        {
            // Resolve the handler from the DI container
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TCommandResult>>();

            // Function to call the handler directly, serves as the last step in the pipeline
            Func<Task<TCommandResult>> handlerCall = () => handler.Handle(command, cancellation);

            // Resolve any registered pipeline behaviors
            var behaviors = _serviceProvider.GetServices<IPipelineBehavior<TCommand, TCommandResult>>();

            // Wrap the handler call in the pipeline behaviors
            foreach (var behavior in behaviors.Reverse())
            {
                var next = handlerCall;
                handlerCall = () => behavior.Handle(command, next, cancellation);
            }

            // Execute the pipeline
            return await handlerCall();
        }
    }

}
