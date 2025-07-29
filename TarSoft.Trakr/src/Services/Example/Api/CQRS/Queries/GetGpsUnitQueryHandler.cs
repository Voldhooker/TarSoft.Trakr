using TarSoft.GpsUnit.Api.Dtos;
using TarSoft.Mediator;
using TarSoft.GpsUnit.Domain;
using Microsoft.EntityFrameworkCore;
using TarSoft.GpsUnit.Infrastructure;
using FluentResults;
using TarSoft.Trakr.Common;

namespace TarSoft.GpsUnit.Api.CQRS.Queries
{

    //public class GetGpsUnitQuery 
    //{
    //    public Guid Id { get; set; }
    //}

    public class GetGpsUnitQuery : IRequest<Result<Domain.GpsUnit>>
    {
        public Guid Id { get; }

        public GetGpsUnitQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetGpsUnitHandler : IQueryHandler<GetGpsUnitQuery, Result<Domain.GpsUnit>>
    {
        private readonly GpsUnitContext _dbContext;
        public GetGpsUnitHandler(GpsUnitContext context)
        {
            _dbContext = context;
        }

        public async Task<Result<Domain.GpsUnit>> Handle(GetGpsUnitQuery query, CancellationToken cancellationToken)
        {
            try
            {
                Domain.GpsUnit? unit = await _dbContext.GpsUnits.FindAsync(query.Id, cancellationToken);
                if (unit is null)
                {
                    return Result.Fail(new NotFoundError());
                }
                return Result.Ok(unit);
            }
            catch (Exception)
            {
                return Result.Fail(new DatabaseError());
            }
        }
    }
}
