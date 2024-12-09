using Microsoft.AspNetCore.Mvc;
using Coffee.Models;
using LinqKit;
using LinqKit.Core;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Controllers
{
    public class ListJpController : Controller
    {
        private ProjectContext _context;

        public ListJpController(ProjectContext dbContext)
        {
            _context = dbContext;
        }

        // -------------------------------日文頁面
        public async Task<IActionResult> AllJp()
        {
            //產品總數量
            var totalcount = _context.ProductJps.Count();
            //所有產地
            var country0 = await _context.ProductJps
                .Select(p => p.Country)
                .Where(c => !string.IsNullOrEmpty(c))
                .ToListAsync();
            var country = country0
                .SelectMany(c => c!.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(s => s.Trim())
                .Distinct()
                .ToArray();
            //所有風味
            var flavor0 = await _context.ProductJps
                .Select(p => p.Flavor)
                .Where(c => !string.IsNullOrEmpty(c))
                .ToListAsync();
            var flavor = flavor0
                .SelectMany(c => c!.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(s => s.Trim())
                .Distinct()
                .ToArray();
            //所有烘焙程度
            var baking = await _context.ProductJps.Select(p => p.Baking)
                .Distinct()
                .ToArrayAsync();
            //所有處理法
            var method = await _context.ProductJps.Select(p => p.Method)
                .Distinct()
                .ToArrayAsync();

            return View("AllJp", new DocLoad()
            {
                TotalCount = totalcount,
                Country = country,
                Flavor = flavor,
                Baking = baking!,
                Method = method!
            });


        }

        // -------------------------------日文查詢
        public async Task<IActionResult> QueryJp(string column, string? category)
        {
            // 分類
            List<ProductJp> query = await (from p in _context.ProductJps
                                         select p).ToListAsync();
            if (column == "原産地")
            {
                query = await (from p in _context.ProductJps
                               where p.Country!.Contains(category!)
                               select p).ToListAsync();
            }
            else if (column == "風味")
            {
                query = await (from p in _context.ProductJps
                               where p.Flavor!.Contains(category!)
                               select p).ToListAsync();
            }
            else if (column == "ドリップコーヒー")
            {
                query = await (from p in _context.ProductJps
                               where p.Category == "ドリップバッグ"
                               select p).ToListAsync();
            }

            // 篩選
            var predicateAnd = PredicateBuilder.New<ProductJp>(true); // AND
            var predicateOr1 = PredicateBuilder.New<ProductJp>(true); // OR
            var predicateOr2 = PredicateBuilder.New<ProductJp>(true); // OR

            // -價格
            var price = Request.Query["price"].ToString();
            if (!string.IsNullOrEmpty(price))
            {
                var min = Convert.ToInt32(price.Split('#')[0]);
                var max = Convert.ToInt32(price.Split('#')[1]);
                predicateAnd = predicateAnd.And(p => p.Price >= min);
                predicateAnd = predicateAnd.And(p => p.Price <= max);
            }

            // -烘焙程度
            var baking = Request.Query["baking"].ToString();
            if (!string.IsNullOrEmpty(baking))
            {
                string[] bakingArr = baking.Split('#');
                foreach (string b in bakingArr)
                {
                    predicateOr1 = predicateOr1.Or(p => p.Baking == b);
                }
                predicateAnd = predicateAnd.And(predicateOr1);
            }

            // -處理法
            var method = Request.Query["method"].ToString();
            if (!string.IsNullOrEmpty(method))
            {
                string[] methodArr = method.Split('#');
                foreach (string m in methodArr)
                {
                    predicateOr2 = predicateOr2.Or(p => p.Method == m);
                }
                predicateAnd = predicateAnd.And(predicateOr2);
            }

            // -味道
            var fragrance = Request.Query["fragrance"].ToString();
            if (!string.IsNullOrEmpty(fragrance))
            {
                int fragranceInt = Convert.ToInt32(fragrance);
                predicateAnd = predicateAnd.And(p => p.Fragrance == fragranceInt);
            }
            var sour = Request.Query["sour"].ToString();
            if (!string.IsNullOrEmpty(sour))
            {
                var sourInt = Convert.ToInt32(sour);
                predicateAnd = predicateAnd.And(p => p.Sour == sourInt);
            }
            var bitter = Request.Query["bitter"].ToString();
            if (!string.IsNullOrEmpty(bitter))
            {
                var bitterInt = Convert.ToInt32(bitter);
                predicateAnd = predicateAnd.And(p => p.Bitter == bitterInt);
            }
            var sweet = Request.Query["sweet"].ToString();
            if (!string.IsNullOrEmpty(sweet))
            {
                var sweetInt = Convert.ToInt32(sweet);
                predicateAnd = predicateAnd.And(p => p.Sweet == sweetInt);
            }
            var strong = Request.Query["strong"].ToString();
            if (!string.IsNullOrEmpty(strong))
            {
                var strongInt = Convert.ToInt32(strong);
                predicateAnd = predicateAnd.And(p => p.Strong == strongInt);
            }
            var query2 = query.Where(predicateAnd);

            // 排序
            string sort = Request.Query["sort"].ToString();
            var query3 = query2.OrderBy(p => p.Id);
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort == "asc")
                {
                    query3 = query2.OrderByDescending(p => p.Price);
                }
                else if (sort == "desc")
                {
                    query3 = query2.OrderBy(p => p.Price);
                }
            }

            // 分頁
            string page = Request.Query["page"].ToString();
            int pageInt = (string.IsNullOrEmpty(page)) ? 1 : Convert.ToInt32(page);
            string item = Request.Query["item"].ToString();
            int itemInt = (string.IsNullOrEmpty(item)) ? 12 : Convert.ToInt32(item);
            var query4 = query3.Skip((pageInt - 1) * itemInt).Take(itemInt).ToList();


            return Json(new { Products = query4, TotalCount = query2.Count() });
        }

        // -------------------------------日文Modal
        [HttpPost]
        public async Task<IActionResult> ShowProductModalJp(string proID)
        {
            var query = (from p in _context.ProductJps
                         where p.ProductId == proID
                         select p).SingleOrDefaultAsync();
            return PartialView("_PartialProductModalJp", await query);
        }
    }    
}
