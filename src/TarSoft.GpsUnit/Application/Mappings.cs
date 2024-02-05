using TarSoft.GpsUnit.Api.Dtos;

namespace TarSoft.GpsUnit.Application
{
    public static class Mappings
    {
        public static GpsUnitDto MapToDto(this Domain.GpsUnit unit)
        {
            return new GpsUnitDto(unit.Id, unit.CustomerId, unit.Name, unit.Description);
        }

        //Create mapping for list of GpsUnit
        public static IEnumerable<GpsUnitDto> MapToDto(this IEnumerable<Domain.GpsUnit> units)
        {
            return units.Select(MapToDto);
        }

    }
}
