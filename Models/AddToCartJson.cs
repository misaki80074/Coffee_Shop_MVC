using Coffee.Controllers;

namespace Coffee.Models
{
    public class AddToCartJson
    {
        public Cartheater Hmodel { get; set; }             // 購物車標題資料
        public List<CartdetailDTO> Dmodels { get; set; }   // 多筆購物車明細資料
    }
}
