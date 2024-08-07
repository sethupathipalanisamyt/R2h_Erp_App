﻿using System;
using System.Collections.Generic;

namespace R2h_Erp_App.DbModels;

public partial class Customer
{
    public int CustomersId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdateedOn { get; set; }

    public bool Isdeleted { get; set; }

    public virtual ICollection<OrderTab> OrderTabs { get; set; } = new List<OrderTab>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
