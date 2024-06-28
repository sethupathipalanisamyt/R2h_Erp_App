using System.ComponentModel.DataAnnotations;
namespace R2h_Erp_App.Models
{
    public class CustomerVM
    {
        public int CustomersId { get; set; }

        public string Name { get; set; } = null!;
        [EmailAddress]
        [Required]

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public bool IsActive { get; set; }

       
    }
}
