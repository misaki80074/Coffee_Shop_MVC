namespace Coffee.Models
{
    public class CartItemDto
    {
        public string ProductId { get; set; }
        public string CartId { get; set; }
        public short CartItemId { get; set; }
        public string name { get; set; }
        public int qty { get; set; }
        public decimal price { get; set; }
        public int? total { get; set; }
        public string image { get; set; }
    }
}
