using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace R2h_Erp_App.Models
{
    public class OrdertabVM
    {
        [Required]
        public string? OrderNumber { get; set; }
        [Required]
        public int CustomerId { get; set; }
      
        public DateTime OrderDate { get; set; }
        [Required]
        public decimal? SubTotal { get; set; }
        [Required]
        [DisplayName("Discount($-Rs)")]
        public decimal? Discount { get; set; }
        [Required]
        public decimal? ShippingFee { get; set; }
        [Required]
        public decimal? NetAmount { get; set; }

        public int? StatusId { get; set; }
        
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public bool IsDeleted { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
