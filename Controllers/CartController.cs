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
        [AuthFilter]
        public IActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        [Route("Cart/Payment")]
        [AuthFilter]
        public IActionResult Payment(IFormCollection form)
        {
            ViewBag.CartId = form["CartID"].ToList();
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
            //var query = _context.Admlookups.ToList();
            var query = _context.Admlookups;

            var userdescription = _context.Customers
                .Where(o => o.UserId == HttpContext.Session.GetString("userid"));

            var USER = new USERANDADMLOOKUP
            {
                adm = query.ToList(),
                user = userdescription.ToList()
            };

            return View(USER);
        }

        public IActionResult shopping_cart()
        {
            return View();
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


        // 處理購物車資料庫 當用戶在登入時合併購物車(localstorage 與 DB 的融合，一樣的累加)
        [HttpPost]
        [Route("/Cart/AddToCartOne")]
        public async Task<IActionResult> AddToCartOne([FromBody] AddToCartJson cart)
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

            // 資料庫帶出
            var existingCartItems = _context.Cartdetails
                .Where(cd => cd.CartId == cartId)
                .ToList();

            foreach (var item in cart.Dmodels)
            {
                // 檢查是否有相同產品存在於購物車，產品編號 and 狀態Y
                var existingItem = existingCartItems
                    .FirstOrDefault(cd => cd.ProductId == item.productID && cd.Status == "Y");

                if (existingItem != null)
                {
                    // 如果產品已存在，更新數量和總價
                    existingItem.Qty = item.Qty;
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
            // 4.保存變更至資料庫
            // 保存所有變更
            await _context.SaveChangesAsync();
            return Ok();
        }
// --------------------------------------------------------------------------------
// --------------------------------------------------------------------------------








        // 傳送 訂單編號 至 Order的 VIEW
        [HttpPost]
        [Route("Cart/OrderFormData")]
        [AuthFilter]
        public async Task<IActionResult> OrderFormData(IFormCollection form)
        {
            string? OrderNO = _dbcn.ItemNO("O");
            // 異步操作
            await Task.Run(() =>
            {
                string? Commet = form["comment"];
                if (Commet.Length == 0)
                {
                    Commet = null; // 轉型
                }
                string sql = $"INSERT INTO ORDERHEADER(OrderId,CustomerID,OrderDate,Total,Name,Mail,Phone,Comment,CreateDate,UpdateDate,Payment,Status,ShipStatus,ShippingMethod,address) VALUES ('{OrderNO}','{HttpContext.Session.GetString("CustomerId")}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{form["totle"]}','{form["name"]}','{form["email"]}','{form["phone"]}',N'{Commet}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{form["paymentmethod"]}','NEW','NEW','{form["transportmethod"]}',N'{form["address"]}')";
                _dbcn.Insert(sql);
                for (int i = 0; i < Request.Form["itemNO"].Count; i++)
                {
                    // 依項目總數決定執行次數，優先級
                    sql = $"INSERT INTO ORDERDETAIL(OrderId,OrderItem,ProductId,Qty,UnitPrice,Totle,CreateDate,UpdateDate) VALUES ('{OrderNO}','{form["itemNO"][i]}','{form["itemProuductID"][i]}','{form["itemQTY"][i]}','{form["itemPrice"][i]}','{form["itemTotle"][i]}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' )";
                    _dbcn.Insert(sql);
                }
                string updateCart = $"UPDATE CARTHEATER SET Status = 'N' WHERE CartId = '{form["CartID"]}'";
                _dbcn.Insert(updateCart);
            });
            return RedirectToAction("Order", new { orderNo = OrderNO });
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
        [Route("/Product/get")]
        public IActionResult GetCart(string userId)
        {
            // 查詢購物車標題
            var cartHeader = _context.Cartheaters
                .FirstOrDefault(ch => ch.UserId == userId && ch.Status == "Y");
            // 查詢購物車詳細項目
            var cartItems = _context.VCartProducts
                .Where(cd => cd.CartId == cartHeader.CartId && cd.DStatus == "Y")
                .Select(cd => new CartItemDto
                {
                    CartId = cd.CartId,
                    ProductId = cd.ProductId,
                    CartItemId = cd.CartItemId,
                    name = cd.ProductName,
                    qty = (short)cd.Qty,
                    price = (decimal)cd.UnitPrice,
                    total = cd.TotalPrice,
                    image = cd.Img
                })
                .ToList();
            // 確認 cartItems 是否有內容
            if (cartItems == null)
            {
                return Ok(new {
                    success = false, message = "購物車為空", Items = new List<object>() 
                });
            }
            // 傳回 JSON
            return Ok(new
            {
                success = true,
                //CartId = cartItems,
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
            return RedirectToAction("shopping_cart", "Cart");
        }


        [HttpGet]
        public IActionResult Order(string orderNo)
        {
            var OrderFilter = _context.VOrderheaderOrderdetails.Where(o => o.OrderId == orderNo);
            var OrderHistory = _context.VOrderHistories.Where(oh => oh.OrderId == orderNo).OrderBy(oid => oid.Updatedate);
            ViewBag.OrderCount = OrderHistory.Count(); // 計算筆數，用於控制td大小
            var Order_New_History = new OrderHistory
            {
                OrderALL = OrderFilter.ToList(),
                Orderhistory = OrderHistory.ToList()
            };
            return View(Order_New_History);
        }


        public IActionResult TRY()
        {
            ViewBag.aaa = _dbcn.ItemNO("C");
            return View();
        }

        // 711 操作
        [HttpPost]
        public IActionResult TRY([FromForm] StoreData storeData) 
        {
            // 獲取門市回傳資料
            string? storeId = storeData.StoreId;           // 門市代碼
            string? storeName = storeData.StoreName;       // 門市名稱
            string? storeAddress = storeData.StoreAddress; // 門市地址
            ViewBag.StoreNAME = storeName;
            ViewBag.StoreADD = storeAddress;
            return View();
        }




    }

    //public class AddOrderJson 
    //{
    //    public Orderheader oh { get; set; }
    //    public List<Orderdetail> od { get; set; }
    //}

    //public class OrderALL
    //{
    //    public Orderheader oh { get; set; }
    //    public List<Orderdetail> od { get; set; }
    //}


}

