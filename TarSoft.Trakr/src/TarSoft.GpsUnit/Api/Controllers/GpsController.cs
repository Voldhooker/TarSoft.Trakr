using Asp.Versioning;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TarSoft.GpsUnit.Api.CQRS.Commands;
using TarSoft.GpsUnit.Api.CQRS.Queries;
using TarSoft.GpsUnit.Api.Dtos;
using TarSoft.GpsUnit.Application;
using TarSoft.Mediator;
using TarSoft.Trakr.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TarSoft.GpsUnit.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class GpsController : ControllerBase
    {
        //private readonly IQueryHandler<GetAllGpsUnitsQuery, Result<IEnumerable<Domain.GpsUnit>>> _getAllUnitsQueryHandler;
        //private readonly IQueryHandler<GetGpsUnitQuery, Result<Domain.GpsUnit>> _getSpecificUnitQueryHandler;

        private readonly IMediator _mediator;


        public GpsController(IMediator mediator)
        {
            this._mediator = mediator;
        }



        //public GpsController(IQueryHandler<GetAllGpsUnitsQuery, Result<IEnumerable<Domain.GpsUnit>>> getAllUnitsQueryHandler, IQueryHandler<GetGpsUnitQuery, Result<Domain.GpsUnit>> getSpecificUnitQueryHandler)
        //{
        //    _getAllUnitsQueryHandler = getAllUnitsQueryHandler;
        //    _getSpecificUnitQueryHandler = getSpecificUnitQueryHandler;
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GpsUnitDto>>> Get(CancellationToken ct)
        {
            //var unitsResult = await _getAllUnitsQueryHandler.Handle(new GetAllGpsUnitsQuery(), ct);
            var unitsResult = await _mediator.Send(new GetAllGpsUnitsQuery(), ct);

            if (unitsResult.IsFailed)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A database error occurred.");
            }   
            return Ok(unitsResult.Value.MapToDto());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GpsUnitDto>> GetByKeyAsync(Guid id, CancellationToken ct)
        {
            //var unitResult = await _getSpecificUnitQueryHandler.Handle(new GetGpsUnitQuery { Id = id }, ct);

            var unitResult = await _mediator.Send(new GetGpsUnitQuery(id), ct);

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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateGpsUnitDto updateDto, CancellationToken ct)
        {
            var updateResult = await _mediator.Send(new UpdateGpsUnitCommand(id, updateDto), ct);

            if (updateResult.IsSuccess)
            {
                // If the update is successful, return NoContent or Ok depending on your API design
                return NoContent(); // or Ok(updateResult) if you want to return the updated entity
            }
            else
            {
                if (updateResult.Errors.Any(e => e is DatabaseError))
                {
                    // Handle the scenario where a DatabaseError occurred
                    return StatusCode(500, "A database error occurred during the update.");
                }
                else if (updateResult.Errors.Any(e => e is NotFoundError))
                {
                    // Handle the scenario where the GPS unit to update was not found
                    return NotFound("The GPS unit to update was not found.");
                }
                else
                {
                    // Handle other errors
                    return BadRequest(updateResult.Errors); // Assuming errors are client-correctable
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken ct)
        {
            var deleteResult = await _mediator.Send(new DeleteGpsUnitCommand(id), ct);

            if (deleteResult.IsSuccess)
            {
                // If the deletion is successful, return NoContent (204)
                return NoContent();
            }
            else
            {
                if (deleteResult.Errors.Any(e => e is DatabaseError))
                {
                    // Handle the scenario where a DatabaseError occurred
                    return StatusCode(500, "A database error occurred during the deletion.");
                }
                else if (deleteResult.Errors.Any(e => e is NotFoundError))
                {
                    // Handle the scenario where the GPS unit to delete was not found
                    return NotFound("The GPS unit to delete was not found.");
                }
                else
                {
                    // Handle other errors
                    return BadRequest(deleteResult.Errors); // Assuming errors are client-correctable
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateGpsUnitDto createDto, CancellationToken ct)
        {
            var command = new AddGpsUnitCommand(createDto);
            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
            {
                var createdUnit = result.Value; // Assuming the result wraps the newly created GpsUnit entity
                return CreatedAtAction(nameof(GetByKeyAsync), new { id = createdUnit.Id }, createdUnit); // Adjust according to your Get method
            }
            else
            {
                //if (result.Errors.Any(e => e is ValidationFailureError))
                //{
                //    // Handle validation failure scenario
                //    return BadRequest(result.Errors);
                //}
                if (result.Errors.Any(e => e is DatabaseError))
                {
                    // Handle the scenario where a DatabaseError occurred
                    return StatusCode(500, "A database error occurred.");
                }
                else
                {
                    // Handle other errors
                    return BadRequest(result.Errors); // Assuming errors are client-correctable
                }
            }
        }

       
    }
}
