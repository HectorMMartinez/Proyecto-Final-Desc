namespace ProyectoFinalParalela.Models
{
    public class SalesRecord
    {
        public string order_id { get; set; } = string.Empty;
        public int order_item_id { get; set; }
        public string product_id { get; set; } = string.Empty;
        public string seller_id { get; set; } = string.Empty;
        public DateTime shipping_limit_date { get; set; }
        public decimal price { get; set; }
        public decimal freight_value { get; set; }
        public int quantity { get; set; }
        public string product_category_name { get; set; } = string.Empty;
        public decimal payment_value { get; set; }
        public string customer_state { get; set; } = string.Empty;
        public DateTime order_purchase_timestamp { get; set; }
    }
}
