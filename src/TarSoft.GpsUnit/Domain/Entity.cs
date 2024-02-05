namespace TarSoft.GpsUnit.Domain
{
    public abstract class Entity : IEquatable<Entity>
    {
        public Guid Id { get; init; }
        public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } 


        protected Entity(Guid id)
        {
            this.Id = id;
        }

        public bool Equals(Entity? other)
        {
            return this.Id  == other?.Id;
        }
    }
}
