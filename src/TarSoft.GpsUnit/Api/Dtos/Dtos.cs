using System.ComponentModel.DataAnnotations;

namespace TarSoft.GpsUnit.Api.Dtos
{
    public sealed record GpsUnitDto(Guid Id, Guid? CustomerId, string Name, string Description);
    public sealed record CreateGpsUnitDto([Required]Guid Id, Guid? CustomerId, [Required]string Name, [Required]string Description);
    public sealed record UpdateGpsUnitDto(Guid Id, [Required] string Name, [Required] string Description);
    public sealed record DeleteGpsUnitDto(Guid Id, Guid CustomerId);

}
