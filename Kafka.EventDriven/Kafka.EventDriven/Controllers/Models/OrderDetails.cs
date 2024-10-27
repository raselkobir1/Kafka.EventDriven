using System.ComponentModel.DataAnnotations;

namespace Kafka.EventDriven.Controllers.Models
{
    public class OrderDetails
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
    }
}
