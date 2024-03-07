using FluentResults;
using Microsoft.AspNetCore.Mvc;
using TarSoft.GpsUnit.Api.Dtos;
using TarSoft.GpsUnit.Application;
using TarSoft.GpsUnit.Infrastructure;
using TarSoft.Mediator;
using TarSoft.Trakr.Common;

namespace TarSoft.GpsUnit.Api.CQRS.Commands
{
    public class AddGpsUnitCommand : IRequest<Result<Domain.GpsUnit>>
    {
        public CreateGpsUnitDto CreateDto { get; }

        public AddGpsUnitCommand(CreateGpsUnitDto createDto)
        {
            CreateDto = createDto;
        }

        public class AddGpsUnitHandler : IRequestHandler<AddGpsUnitCommand, Result<Domain.GpsUnit>>
        {
            private readonly GpsUnitContext _dbContext;

            public AddGpsUnitHandler(GpsUnitContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Result<Domain.GpsUnit>> Handle(AddGpsUnitCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var newUnit = command.CreateDto.MapToEntity(); // Map the CreateGpsUnitDto to a GpsUnit entity

                    _dbContext.GpsUnits.Add(newUnit);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    return Result.Ok(newUnit); // Return the newly created GpsUnit entity
                }
                catch (Exception ex)
                {
                    // Log the exception if necessary
                    return Result.Fail<Domain.GpsUnit>(new DatabaseError());
                }
            }

           

        }

    }

}
