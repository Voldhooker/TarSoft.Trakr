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

    public class NotFoundError : Error
    {
        public NotFoundError()
            : base($"The requested resource was not found")
        {
        }
    }
}
