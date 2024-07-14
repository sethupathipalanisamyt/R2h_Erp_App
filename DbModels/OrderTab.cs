using System;
using System.Collections.Generic;

namespace R2h_Erp_App.DbModels;

public partial class OrderTab
{
    public int OrderId { get; set; }

    public string? OrderNumber { get; set; }

    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal? Discount { get; set; }

    public decimal? ShippingFee { get; set; }

    public decimal? NetAmount { get; set; }

    public int? StatusId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderItemTab> OrderItemTabs { get; set; } = new List<OrderItemTab>();

    public virtual StatusTab? Status { get; set; }
}
