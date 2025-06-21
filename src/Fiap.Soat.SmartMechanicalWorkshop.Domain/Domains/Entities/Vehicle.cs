namespace AutoRepairShopManagementSystem.Domains.Entities
{
    public record Vehicle : Entity
    {
        public string LicensePlate { get; set; }
        public DateOnly ManufactureYear { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public Guid ClientId { get; set; }
    }
}
