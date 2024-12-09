using Microsoft.AspNetCore.Mvc;

namespace Coffee.Models
{
    public class StoreData
    {
        [FromForm(Name = "storeid")]
        public string? StoreId { get; set; }

        [FromForm(Name = "storename")]
        public string? StoreName { get; set; }

        [FromForm(Name = "storeaddress")]
        public string? StoreAddress { get; set; }
    }
}
