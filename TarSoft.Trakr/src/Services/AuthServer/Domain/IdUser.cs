using Microsoft.AspNetCore.Identity;

namespace TarSoft.Trakr.Identity.Domain
{
    public class IdUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
