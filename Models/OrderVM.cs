using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace R2h_Erp_App.Models
{
    public class OrderVM
    {
        public int OrderId { get; set; }
        [Required]
        [ForeignKey("Customers")]
        public int CustomersId { get; set; }
        [Required]
        [ForeignKey("Products")]
        public int ProductId { get; set; }

        public DateTime OrderDate { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Amount { get; set; }

        public decimal TotalAmount { get; set; }

    }
}
