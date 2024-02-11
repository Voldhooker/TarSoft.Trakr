using TarSoft.Mediator;
using Microsoft.EntityFrameworkCore;
using TarSoft.GpsUnit.Infrastructure;
using FluentResults;

namespace TarSoft.GpsUnit.Api.CQRS.Queries
{

    //public class GetAllGpsUnitsQuery : IRequest<Result<List<Domain.GpsUnit>>>
    //{ }


    //public class GetAllGpsUnitsHandler : IQueryHandler<GetAllGpsUnitsQuery, Result<IEnumerable<Domain.GpsUnit>>>
    //{

    //    private readonly GpsUnitContext _dbContext;
    //    public GetAllGpsUnitsHandler(GpsUnitContext context)
    //    {
    //        _dbContext = context;
    //    }

    //    public async Task<Result<IEnumerable<Domain.GpsUnit>>> Handle(GetAllGpsUnitsQuery query, CancellationToken cancellationToken)
    //    {

    //        //return new List<Domain.GpsUnit>(); // Return users
    //        //var units = await _dbContext.GpsUnits.ToListAsync(cancellationToken);
    //        //return new List<Domain.GpsUnit>(); // Return users

    //        var units = await _dbContext.GpsUnits.ToListAsync(cancellationToken);
    //        return Result.Ok<IEnumerable<Domain.GpsUnit>>(units);

    //    }
    //}

    public class GetAllGpsUnitsQuery : IRequest<Result<List<Domain.GpsUnit>>>
    {
        // Query definition
    }

    public class GetAllGpsUnitsHandler : IQueryHandler<GetAllGpsUnitsQuery, Result<List<Domain.GpsUnit>>>
    {

        private readonly GpsUnitContext _dbContext;
        public GetAllGpsUnitsHandler(GpsUnitContext context)
        {
            _dbContext = context;
        }

        // Handler implementation
        public async Task<Result<List<Domain.GpsUnit>>> Handle(GetAllGpsUnitsQuery query, CancellationToken cancellationToken)
        {
            var units = await _dbContext.GpsUnits.ToListAsync(cancellationToken);
            return Result.Ok<List<Domain.GpsUnit>>(units);
        }
    }

}
