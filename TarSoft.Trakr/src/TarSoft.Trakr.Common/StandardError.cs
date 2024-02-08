using FluentResults;

namespace TarSoft.Trakr.Common
{
    public class DatabaseError : Error
    {
        public DatabaseError()
            : base($"There was an error with the database")
        {
        }
    }
}
