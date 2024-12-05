namespace Coffee.Models
{
    public class AddOrderJson
    {
        public Orderheader oh { get; set; }
        public List<Orderdetail> od { get; set; }
    }
}
