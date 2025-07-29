using FluentResults;
using TarSoft.GpsUnit.Api.Dtos;
using TarSoft.GpsUnit.Infrastructure;
using TarSoft.Mediator;
using TarSoft.Trakr.Common;

namespace TarSoft.GpsUnit.Api.CQRS.Commands
{
    public class UpdateGpsUnitCommand : IRequest<Result>
    {
        public Guid Id { get; }
        public UpdateGpsUnitDto UpdateDto { get; }

        public UpdateGpsUnitCommand(Guid id, UpdateGpsUnitDto updateDto)
        {
            Id = id;
            UpdateDto = updateDto;
        }
    }

    public class UpdateGpsUnitHandler : IRequestHandler<UpdateGpsUnitCommand, Result>
    {
        private readonly GpsUnitContext _dbContext;

        public UpdateGpsUnitHandler(GpsUnitContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(UpdateGpsUnitCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var unit = await _dbContext.GpsUnits.FindAsync(new object[] { command.Id }, cancellationToken);
                if (unit == null)
                {
                    return Result.Fail(new NotFoundError());
                }

                // Map UpdateDto to the unit entity here
                // For example: unit.Name = command.UpdateDto.Name;
                // Remember to validate UpdateDto contents as needed

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
