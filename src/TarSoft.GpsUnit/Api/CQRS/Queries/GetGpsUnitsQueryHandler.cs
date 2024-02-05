using TarSoft.GpsUnit.Api.Dtos;
using TarSoft.Mediator;
using TarSoft.GpsUnit.Domain;
using Microsoft.EntityFrameworkCore;
using TarSoft.GpsUnit.Infrastructure;
using FluentResults;

namespace TarSoft.GpsUnit.Api.CQRS.Queries
{

    public class GetAllGpsUnitsQuery { }


    public class GetAllGpsUnitsHandler : IQueryHandler<GetAllGpsUnitsQuery, Result<IEnumerable<Domain.GpsUnit>>>
    {

        //private readonly GpsUnitContext _dbContext;
        //public GetAllGpsUnitsHandler(GpsUnitContext context)
        //{
        //    _dbContext = context;
        //}

        public async Task<Result<IEnumerable<Domain.GpsUnit>>> Handle(GetAllGpsUnitsQuery query, CancellationToken cancellationToken)
        {

            return new List<Domain.GpsUnit>(); // Return users
            //var units = await _dbContext.GpsUnits.ToListAsync(cancellationToken);
            //return new List<Domain.GpsUnit>(); // Return users
        }
    }
}
