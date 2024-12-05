using Coffee.Models;

namespace Coffee.Models
{
    public class OrderHistory
    {
        public List<VOrderheaderOrderdetail> OrderALL { get; set; }
        public List<VOrderHistory> Orderhistory { get; set; }
    }
}
