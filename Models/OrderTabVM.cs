namespace R2h_Erp_App.Models
{
    public class OrderTabVM
    {
        public int OrderId { get; set; }

        public string? OrderNumber { get; set; }

        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal? SubTotal { get; set; }

        public decimal? Discount { get; set; }

        public decimal? ShippingFee { get; set; }

        public decimal? NetAmount { get; set; }

        public int StatusId { get; set; }
    }
}
