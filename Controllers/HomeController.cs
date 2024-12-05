using Coffee.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Coffee.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProjectContext _context;

        public HomeController(ILogger<HomeController> logger, ProjectContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IndexEn()
        {
            return View();
        }

        public IActionResult IndexJp()
        {
            return View();
        }

        // 中文資料
        [HttpPost]
        public JsonResult ProductData(string country)
        {
            Dictionary<string, string> countryDic = new Dictionary<string, string>();
            countryDic.Add("Guatemala", "瓜地馬拉");
            countryDic.Add("Colombia", "哥倫比亞");
            countryDic.Add("Brazil", "巴西");
            countryDic.Add("Ethiopia", "衣索比亞");
            countryDic.Add("Kenya", "肯亞");

            string chineseCountry = countryDic.FirstOrDefault(x => x.Key == country).Value;

            //var query = (from o in _context.Products
            //             where o.Id == (from i in _context.Products
            //                            where i.Country == "巴西"
            //                            select i.Id).Max()
            //             select new Product
            //             {
            //                 Id = o.Id,
            //                 ProductId = o.ProductId,
            //                 ProductName = o.ProductName
            //             }).ToList();


            string? firstSearch = (from p in _context.Products
                                   where p.Country == chineseCountry
                                   select p.ProductId).Max();

            var result = (from p in _context.Products
                          where p.ProductId == firstSearch
                          select new Product { ProductName = p.ProductName, Img = p.Img, Country = p.Country }).FirstOrDefault();

            string s = JsonConvert.SerializeObject(result);

            Product json = JsonConvert.DeserializeObject<Product>(s)!;

            return Json(json);
        }

        // 英文資料
        [HttpPost]
        public JsonResult ProductDataEn(string country)
        {
            string? firstSearch = (from p in _context.ProductEns
                                   where p.Country == country
                                   select p.ProductId).Max();

            var result = (from p in _context.ProductEns
                          where p.ProductId == firstSearch
                          select new Product { ProductName = p.ProductName, Img = p.Img, Country = p.Country }).FirstOrDefault();

            string s = JsonConvert.SerializeObject(result);

            Product json = JsonConvert.DeserializeObject<Product>(s)!;

            return Json(json);
        }

        // 日文資料
        [HttpPost]
        public JsonResult ProductDataJp(string country)
        {
            Dictionary<string, string> countryDic = new Dictionary<string, string>();
            countryDic.Add("Guatemala", "グアテマラ");
            countryDic.Add("Colombia", "コロンビア");
            countryDic.Add("Brazil", "ブラジル");
            countryDic.Add("Ethiopia", "エチオピア");
            countryDic.Add("Kenya", "ケニア");

            string chineseCountry = countryDic.FirstOrDefault(x => x.Key == country).Value;

            string? firstSearch = (from p in _context.ProductJps
                                   where p.Country == chineseCountry
                                   select p.ProductId).Max();

            var result = (from p in _context.ProductJps
                          where p.ProductId == firstSearch
                          select new Product { ProductName = p.ProductName, Img = p.Img, Country = p.Country }).FirstOrDefault();

            string s = JsonConvert.SerializeObject(result);

            Product json = JsonConvert.DeserializeObject<Product>(s)!;

            return Json(json);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
