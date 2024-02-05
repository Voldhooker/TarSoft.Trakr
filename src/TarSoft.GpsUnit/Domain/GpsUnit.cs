
namespace TarSoft.GpsUnit.Domain
{
    public class GpsUnit : Entity
    {
        public Guid? CustomerId { get; private set; }
        public Guid ApiKey { get; init; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;


        public GpsUnit(Guid id, Guid apiKey, string name, string description) : base(id)
        {
            ApiKey = apiKey;
            Name = name;
            Description = description;
        }

        public void SetCustomer(Guid customerId)
        {

            if(CustomerId.HasValue)
            {
                throw new DomainException("Customer already set");
            }
            CustomerId = customerId;
        }   
    }
}
