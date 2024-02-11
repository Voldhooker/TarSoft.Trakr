using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarSoft.Mediator
{
    //https://cezarypiatek.github.io/post/why-i-dont-use-mediatr-for-cqrs/  
    public interface IQueryHandler<in TQuery, TQueryResult>
    {
        Task<TQueryResult> Handle(TQuery query, CancellationToken cancellation);
    }

    public interface IQueryDispatcher
    {
        Task<TQueryResult> Dispatch<TQuery, TQueryResult>(TQuery query, CancellationToken cancellation);
    }

    public interface ICommandHandler<in TCommand, TCommandResult>
    {
        Task<TCommandResult> Handle(TCommand command, CancellationToken cancellation);
    }

    public interface ICommandDispatcher
    {
        Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation);
    }

    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }

    public interface IRequestHandler<in TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }

    // Represents a request with a response
    public interface IRequest<TResponse> { }

    // Represents a request without a specific response (e.g., for commands that do not return a result)
    public interface IRequest : IRequest<Unit>
    {
        // Unit is a value indicating an absence of a specific return type, similar to void but usable in a generic context
    }

    public struct Unit
    {
        public static readonly Unit Value = new Unit();
    }

}
