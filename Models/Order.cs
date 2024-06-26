using System;
using System.Collections.Generic;

namespace R2h_Erp_App.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomersId { get; set; }

    public int ProductId { get; set; }

    public DateTime OrderDate { get; set; }

    public int Quantity { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdateedOn { get; set; }

    public bool Isdeleted { get; set; }

    public virtual Customer Customers { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
