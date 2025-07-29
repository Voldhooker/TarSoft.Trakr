using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TarSoft.Trakr.Identity.Domain;

namespace TarSoft.Trakr.Identity.Data
{
    public class IdContext : IdentityDbContext<IdUser>
    {
        public IdContext(DbContextOptions<IdContext> options) : base(options)
        {
        }
    }

}
