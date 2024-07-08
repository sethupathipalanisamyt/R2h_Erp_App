using System.ComponentModel.DataAnnotations;
namespace R2h_Erp_App.Models
{
    public class ProductVM
    {
        public int ProductsId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Code { get; set; } = null!;

        public bool IsActive { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }


    }
}
