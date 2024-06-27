using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace R2h_Erp_App.Models;

public partial class Customer
{
    [Key]
    public int CustomersId { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [EmailAddress]
    public string Email { get; set; } = null!;
    [MaxLength(10)]
    public string Phone { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdateedOn { get; set; }

    public bool Isdeleted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
