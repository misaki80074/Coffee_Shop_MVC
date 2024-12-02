using Coffee.Models;

namespace Coffee.Views.Account
{
    public class OrderDetailViewModel
    {
        public string OrderId { get; set; } = null!;

        public short OrderItem { get; set; }

        public string? ProductId { get; set; }

        public string? ProductName { get; set; }

        public short? Qty { get; set; }

        public string? Uom { get; set; }

        public short? UnitPrice { get; set; }
    }
}
