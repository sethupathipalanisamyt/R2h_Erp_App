using System;
using System.Collections.Generic;

namespace R2h_Erp_App.DbModels;

public partial class StatusTab
{
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<OrderTab> OrderTabs { get; set; } = new List<OrderTab>();
}
