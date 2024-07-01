using System.ComponentModel.DataAnnotations;

namespace R2h_Erp_App.Models
{
    public class OrderVM
    {
        public int OrderId { get; set; }
        [Required]
        public int CustomersId { get; set; }
        [Required]
        public int ProductId { get; set; }

        public DateTime OrderDate { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Amount { get; set; }

    }
}
