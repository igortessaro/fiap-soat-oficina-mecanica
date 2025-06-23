using System.ComponentModel.DataAnnotations;

namespace AutoRepairShopManagementSystem.Domains.Entities
{
    public record Entity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
