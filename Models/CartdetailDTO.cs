namespace Coffee.Models
{
    public class CartdetailDTO
    {
        public string productID { get; set; }              // 商品 ID
        public short Qty { get; set; }                     // 數量
        public short UnitPrice { get; set; }               // 單價
    }
}
