using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Coffee.Models;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Coffee;


namespace Coffee.Controllers
{
    public class CartController : Controller
    {
            private readonly DBCNcart _dbcn;
            private readonly ProjectContext _context;

            // 建構函數注入 DBCNcart 和 ProjectContext
            public CartController(DBCNcart dbcn, ProjectContext context)
            {
                _dbcn = dbcn;
                _context = context;
            }

            // Index 方法
            [Route("TestCart")]
            public IActionResult Index()
            {
                // 使用 _dbcn 進行 SQL 查詢操作
                string sql = "SELECT * FROM PRODUCT";
                DataTable dt = _dbcn.SQL(sql);

                // 將 DataTable 轉換成 List<Product>
                List<Product> productList = new List<Product>();
                foreach (DataRow row in dt.Rows)
                {
                    productList.Add(new Product
                    {
                        ProductId = row["ProductId"].ToString(),
                        ProductName = row["ProductName"].ToString()
                        // 若需要額外欄位，根據資料庫欄位名進行擴充
                    });
                }

                // 傳遞產品清單到 View
                return View(productList);
            }

        [Route("TestCommodity")]
        public IActionResult Commodity()
        {
            // 使用 _dbcn 進行 SQL 查詢操作
            string sql = "SELECT * FROM PRODUCT";
            DataTable dt = _dbcn.SQL(sql);

            // 將 DataTable 轉換成 List<Product>
            List<Product> productList = new List<Product>();
            foreach (DataRow row in dt.Rows)
            {
                productList.Add(new Product
                {
                    ProductId = row["ProductId"].ToString(),
                    ProductName = row["ProductName"].ToString(),
                    Price = (short)row["Price"],  // 保持使用 short 型別
                    Img = row["Img"].ToString()
                });
            }

            // 傳遞商品清單到 View
            return View(productList);
        }

        [HttpGet]
        public IActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        [Route("Cart/Payment")]
        public IActionResult Payment(IFormCollection form)
        {
            // 把表單資料放入 ViewBag，目前出現空集合，待修
            ViewBag.ID = form["itemNO"].ToList();
            ViewBag.ProductID = form["itemId"].ToList();
            ViewBag.Name = form["itemNane"].ToList();
            ViewBag.QTY = form["itemQTY"].ToList();
            ViewBag.Price = form["itemprice"].ToList();
            ViewBag.itemtotle = form["itemtotal"].ToList();
            ViewBag.Alltotle = form["totalprice"];
            ViewBag.IMG = form["imgsrc"].ToList();

            

            // 回傳視圖
            return View();
        }

        public IActionResult shopping_cart()
        {
            return View();
        }

        [HttpPost]
        [Route("Cart/OrderFormData")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderFormData(IFormCollection form)
        {
            // 異步操作
            await Task.Run(() =>
            {
                // 生成訂單編號 (OrderNO)
                string? OrderNO = _dbcn.ItemNO("O"); // 確保生成的 OrderNO 是唯一的

                // 組建 SQL 語句並插入 OrderHeader 資料
                string sqlOrderHeader = $"INSERT INTO ORDERHEADER(OrderId, OrderDate, Total, Name, Mail, Phone, Comment, CreateDate, UpdateDate, Payment, Status, ShipStatus) " +
                                        $"VALUES ('{OrderNO}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{form["totle"]}', '{form["name"]}', '{form["email"]}', '{form["phone"]}', " +
                                        $"'{form["comment"]}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{form["paymentmethod"]}', 'N', 'N')";

                // 執行插入 SQL 語句
                _dbcn.Insert(sqlOrderHeader);

                // 插入 OrderDetail 資料
                for (int i = 0; i < Request.Form["itemNO"].Count; i++)
                {
                    string sqlOrderDetail = $"INSERT INTO ORDERDETAIL(OrderId, OrderItem, ProductId, Qty, UnitPrice, Totle, CreateDate, UpdateDate) " +
                                            $"VALUES ('{OrderNO}', '{form["itemNO"][i]}', '{form["itemProuductID"][i]}', '{form["itemQTY"][i]}', '{form["itemPrice"][i]}', '{form["itemTotle"][i]}', " +
                                            $"'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";

                    // 執行每筆 OrderDetail 插入 SQL 語句
                    _dbcn.Insert(sqlOrderDetail);
                }
            });

            // 操作完成後，導向到其他頁面 (例如顯示訂單或返回首頁)
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Cart/LOING")]
        public async Task<IActionResult> LOING(IFormCollection form)
        {
            var newCustomer = new Customer
            {
                CustomerId = form["login"],
                UserId = form["pass"]
            };

            // 新增到資料庫並保存更改
            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();

            return RedirectToAction("TRY");
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartJson cart)
        {
            // 檢查是否已存在有效的購物車標題
            var cartHeader = _context.Cartheaters
                .FirstOrDefault(ch => ch.UserId == cart.Hmodel.UserId && ch.Status == "Y");
            string cartId;

            if (cartHeader != null)
            {
                // 若購物車標題存在，使用現有的購物車編號
                cartId = cartHeader.CartId;
            }
            else
            {
                // 若購物車標題不存在，創建新的購物車標題
                cartId = _dbcn.ItemNO("C");
                var newCartHeader = new Cartheater
                {
                    CartId = cartId,
                    UserId = cart.Hmodel.UserId,
                    CreateDate = DateTime.Now,
                    Status = "Y"
                };
                _context.Cartheaters.Add(newCartHeader);
            }

            // 取得該購物車目前的所有項目
            var existingCartItems = _context.Cartdetails
                .Where(cd => cd.CartId == cartId)
                .ToList();

            foreach (var item in cart.Dmodels)
            {
                // 檢查是否有相同產品存在於購物車
                var existingItem = existingCartItems
                    .FirstOrDefault(cd => cd.ProductId == item.productID);

                if (existingItem != null)
                {
                    // 如果產品已存在，更新數量和總價
                    existingItem.Qty += item.Qty;
                    existingItem.TotalPrice = existingItem.Qty * existingItem.UnitPrice;
                }
                else
                {
                    // 如果產品不存在，新增至購物車
                    var newCartItem = new Cartdetail
                    {
                        CartId = cartId,
                        CartItemId = (short)(existingCartItems.Count + 1), // 自動生成新的 CartItemId
                        ProductId = item.productID,
                        Qty = item.Qty,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.Qty * item.UnitPrice,
                        CreateDate = DateTime.Now,
                        Status = "Y"
                    };
                    _context.Cartdetails.Add(newCartItem);
                    existingCartItems.Add(newCartItem); // 更新清單
                }
            }

            // 保存所有變更
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("/Cart/DeleteCartItem")]
        public async Task<IActionResult> DeleteCartItem([FromBody] SingelCartJson cart)
        {
            // 查找當前有效的購物車標題
            var cartHeader = _context.Cartheaters
                .FirstOrDefault(ch => ch.UserId == cart.Hmodel.UserId && ch.Status == "Y");

            if (cartHeader != null)
            {
                string cartId = cartHeader.CartId;

                // 查找符合條件的購物車項目
                var cartItem = _context.Cartdetails
                    .FirstOrDefault(cd => cd.CartId == cartId && cd.ProductId == cart.Dmodels.ProductId && cd.Status == "Y");

                if (cartItem != null)
                {
                    // 將項目的狀態設為 "N"
                    cartItem.Status = "N";
                    await _context.SaveChangesAsync();
                }
            }

            // 重導回購物車頁面
            return RedirectToAction("ShoppingCart", "Cart");
        }

        [HttpGet]
        [Route("/Cart/GetCart")]
        public IActionResult GetCart(string userId)
        {
            // 查詢購物車標題
            var cartHeader = _context.Cartheaters
                .FirstOrDefault(ch => ch.UserId == userId && ch.Status == "Y");

            if (cartHeader == null)
            {
                // 若找不到購物車標題，回傳空結果
                return Ok(new { success = false, message = "購物車不存在", items = new List<object>() });
            }

            // 查詢購物車詳細項目
            var cartItems = _context.VCartProducts
                .Where(cd => cd.CartId == cartHeader.CartId && cd.DStatus == "Y")
                .Select(cd => new CartItemDto
                {
                    ProductId = cd.ProductId,
                    CartItemId = cd.CartItemId,
                    name = cd.ProductName,
                    qty = (short)cd.Qty,
                    price = (decimal)cd.UnitPrice,
                    total = cd.TotalPrice,
                    //image = cd.Img<---找不到(源專案也是--ZHITAI)
                })
                .ToList();

            // 檢查是否有購物車項目
            if (cartItems == null || !cartItems.Any())
            {
                return Ok(new { success = false, message = "購物車為空", items = new List<object>() });
            }

            // 回傳購物車資訊與項目
            return Ok(new
            {
                success = true,
                CartId = cartHeader.CartId,
                UserId = cartHeader.UserId,
                items = cartItems
            });
        }

        [HttpGet]
        [Route("/Cart/RemoveCart/{userId}")]
        public async Task<IActionResult> RemoveCart(string userId)
        {
            // 查詢該用戶的有效購物車標題
            var removecart = _context.Cartheaters
                .FirstOrDefault(ch => ch.UserId == userId && ch.Status == "Y");

            if (removecart != null)
            {
                // 將購物車標題的狀態設為 N
                removecart.Status = "N";
                await _context.SaveChangesAsync();
            }

            // 重定向到購物車頁面，假設對應控制器與視圖
            return RedirectToAction("ShoppingCart", "Cart");
        }


        public IActionResult Order()
        {
            var query = (from o in _context.VOrderheaderOrderdetails
                             //where o.CustomerId == ""
                         select o).ToList();
            return View(query);
        }



        public IActionResult TRY()
        {
            ViewBag.aaa = _dbcn.ItemNO("C");
            return View();
        }

    }

    public class SingelCartJson 
    {
        public Cartheater? Hmodel { get; set; }
        public Cartdetail? Dmodels { get; set; }
    }

    public class AddOrderJson 
    {
        public Orderheader oh { get; set; }
        public List<Orderdetail> od { get; set; }
    }

    public class AddToCartJson 
    {
        public Cartheater Hmodel { get; set; }             // 購物車標題資料
        public List<CartdetailDTO> Dmodels { get; set; }   // 多筆購物車明細資料
    }

    public class CartdetailDTO 
    {
        public string productID { get; set; }              // 商品 ID
        public short Qty { get; set; }                     // 數量
        public short UnitPrice { get; set; }               // 單價
    }

    public class CartItemDto 
    {
        public string ProductId { get; set; }
        public short CartItemId { get; set; }
        public string name { get; set; }
        public int qty { get; set; }
        public decimal price { get; set; }
        public int? total { get; set; }
        public string image { get; set; }
    }

    public class OrderALL
    {
        public Orderheader oh { get; set; }
        public List<Orderdetail> od { get; set; }
    }

}

