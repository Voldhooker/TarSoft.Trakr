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
}
