using FluentResults;
using TarSoft.GpsUnit.Infrastructure;
using TarSoft.Mediator;
using TarSoft.Trakr.Common;

namespace TarSoft.GpsUnit.Api.CQRS.Commands
{
    public class DeleteGpsUnitCommand : IRequest<Result>
    {
        public Guid Id { get; }

        public DeleteGpsUnitCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteGpsUnitHandler : IRequestHandler<DeleteGpsUnitCommand, Result>
    {
        private readonly GpsUnitContext _dbContext;

        public DeleteGpsUnitHandler(GpsUnitContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(DeleteGpsUnitCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var unit = await _dbContext.GpsUnits.FindAsync(new object[] { command.Id }, cancellationToken);
                if (unit == null)
                {
                    return Result.Fail(new NotFoundError());
                }

                _dbContext.GpsUnits.Remove(unit);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Result.Ok();
            }
            catch (Exception)
            {
                return Result.Fail(new DatabaseError());
            }
        }
    }

}
