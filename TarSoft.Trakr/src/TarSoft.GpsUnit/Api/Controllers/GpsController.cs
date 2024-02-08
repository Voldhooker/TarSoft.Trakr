using Asp.Versioning;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TarSoft.GpsUnit.Api.CQRS.Queries;
using TarSoft.GpsUnit.Api.Dtos;
using TarSoft.GpsUnit.Application;
using TarSoft.Mediator;
using TarSoft.Trakr.Common;

namespace TarSoft.GpsUnit.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class GpsController : ControllerBase
    {
        private readonly IQueryHandler<GetAllGpsUnitsQuery, Result<IEnumerable<Domain.GpsUnit>>> _getAllUnitsQueryHandler;
        private readonly IQueryHandler<GetGpsUnitQuery, Result<Domain.GpsUnit>> _getSpecificUnitQueryHandler;





        public GpsController(IQueryHandler<GetAllGpsUnitsQuery, Result<IEnumerable<Domain.GpsUnit>>> getAllUnitsQueryHandler, IQueryHandler<GetGpsUnitQuery, Result<Domain.GpsUnit>> getSpecificUnitQueryHandler)
        {
            _getAllUnitsQueryHandler = getAllUnitsQueryHandler;
            _getSpecificUnitQueryHandler = getSpecificUnitQueryHandler;
        }

        //[HttpGet]
        //public async Task<IEnumerable<GpsUnitDto>> Get(CancellationToken ct)
        //{
        //    var units = await _getAllUnitsQueryHandler.Handle(new GetAllGpsUnitsQuery(), ct);
        //    return units.MapToDto();
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<GpsUnitDto>> GetByKeyAsync(Guid id)
        {
            var unitResult = await _getSpecificUnitQueryHandler.Handle(new GetGpsUnitQuery { Id = id }, CancellationToken.None);

            if (unitResult.IsSuccess)
            {
                // Handle the success scenario
                return Ok(unitResult);
            }
            else
            {
                // Check for custom DatabaseError
                if (unitResult.Errors.Any(e => e is DatabaseError))
                {
                    // Handle the scenario where a DatabaseError occurred
                    return StatusCode(500, "A database error occurred.");
                }
                else
                {
                    // Handle other errors
                    return NotFound();
                }
            }
        }

        //[HttpPost]
        //public ActionResult<GpsUnitDto> Create(CreateGpsUnitDto dto)
        //{
        //    var newDto = new GpsUnitDto(Guid.NewGuid(), dto.CustomerId, dto.Name, dto.Description, Guid.NewGuid());
        //    dtos.Add(newDto);

        //    return CreatedAtAction(nameof(GetByKey), new { id = newDto.Id }, newDto);
        //}

        //[HttpPut("{id}")]
        //public ActionResult<GpsUnitDto> Update(Guid id, UpdateGpsUnitDto dto)
        //{
        //    var existingDto = dtos.Where(x => x.Id == id).SingleOrDefault();    

        //    if (existingDto is null)
        //    {
        //        return NotFound();
        //    }

        //    var index = dtos.FindIndex(x => x.Id == id);
        //    var updatedItem = existingDto with
        //    {
        //        Name = dto.Name,
        //        Description = dto.Description
        //    };
            
        //    dtos[index] = updatedItem;

        //    return Ok(updatedItem);
        //}

        //[HttpDelete("{id}")]
        //public ActionResult Delete(Guid id)
        //{
        //    var unit = dtos.Where(x => x.Id == id).SingleOrDefault();

        //    if (unit == null)
        //    {
        //        return NotFound();
        //    }

        //    dtos.Remove(unit);

        //    return NoContent();
        //}
    }
}
