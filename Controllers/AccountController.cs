using Microsoft.AspNetCore.Mvc;
using Coffee.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Cryptography;
using System.Text;

namespace Coffee.Controllers
{
    public class AccountController : Controller
    {
        private ProjectContext _context;

        public AccountController(ProjectContext dbContext)
        {
            _context = dbContext;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [AuthFilter]
        public IActionResult Member()
        {
            return View();
        }

        //-------------------------------------------------------------
        
        /// <summary>
        /// 取得帳號名稱
        /// </summary>
        /// <returns>0:沒有帳號，Json(Name):使用者姓名</returns>
        [HttpGet]
        public IActionResult GetUserid()
        {
            string userid = HttpContext.Session.GetString("userid")!;
            if (userid == null)
            {
                return new ContentResult() { Content = "0" };
            }
            else
            {
                string name = _context.Customers.Where(c => c.UserId == userid).Select(c => c.Name).SingleOrDefault()!;
                return Json(new { Name = name , Userid = userid});
            }
        }

        /// <summary>
        /// 將密碼雜湊轉換
        /// </summary>
        /// <param name="password"></param> 前端傳送過來的密碼
        /// <returns>雜湊後的密碼</returns>
        private string HashPassword(string password)
        {
            // try、catch刪掉會 跑出500錯誤
            try
            {
                // 這裡使用 SHA256 進行密碼雜湊，實際應根據需求使用適當的雜湊方法
                using (var sha256 = SHA256.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                    return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                }
            }
            catch (Exception ex)
            {
                // 捕獲記錄錯誤
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="userid"></param> 帳號
        /// <param name="password"></param> 密碼雜湊值
        /// <returns>0:登入成功，1:登入失敗，2:未註冊</returns>
        [HttpPost]
        public IActionResult Login(string userid, string password)
        {
            var member = _context.Customers.SingleOrDefault(m => m.UserId == userid);

            //帳號存在
            if (member != null)
            {
                var n_password = HashPassword(password);
                //密碼正確
                if (member.Password == n_password)
                {
                    HttpContext.Session.SetString("userid", userid);
                    HttpContext.Session.SetString("password", n_password);
                    HttpContext.Session.SetString("CustomerId", member.CustomerId);
                    return new ContentResult() { Content = "0" };
                }
                //密碼不正確
                else
                {
                    return new ContentResult() { Content = "1" };
                }
            }
            //帳號不存在
            else
            {
                return new ContentResult() { Content = "2" };
            }
        }

        /// <summary>
        /// 檢查帳號是否存在
        /// </summary>
        /// <param name="userid"></param> 帳號
        /// <returns>0:帳號已存在，1:無此帳號</returns>
        [HttpPost]
        public IActionResult CheckUserid(string userid)
        {
            var member = _context.Customers.SingleOrDefault(m => m.UserId == userid);

            //帳號不存在
            if (member == null)
            {
                return new ContentResult() { Content = "1" };
            }
            //帳號存在
            else
            {
                return new ContentResult() { Content = "0" };
            }
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="userid"></param> 帳號
        /// <param name="password"></param> 密碼雜湊值
        /// <param name="name"></param> 姓名
        /// <param name="phone"></param> 電話
        /// <param name="email"></param> 信箱
        /// <returns>註冊成功</returns>
        [HttpPost]
        public IActionResult Register([FromForm] string userid, [FromForm] string password, [FromForm] string password2, [FromForm] string name, [FromForm] string phone, [FromForm] string email)
        {
            Console.WriteLine($"Received data: userid={userid}, password={password}, name={name}, phone={phone}, email={email}");
            //產生新的CustomerId
            string customerId = _context.Customers.OrderByDescending(c => c.Id).Select(c => c.CustomerId).FirstOrDefault()!;
            string en = customerId.Substring(0, 4);
            int num = int.Parse(customerId.Substring(4)) + 1;
            string s_num = num.ToString("D8");
            string n_customerId = en + s_num;

            //密碼轉換
            var n_password = HashPassword(password);

            var customer = new Customer()
            {
                CustomerId = n_customerId,
                UserId = userid,
                Password = n_password,
                Name = name,
                Phone = phone,
                Email = email
            };
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return new ContentResult() { Content = "註冊成功" };
        }

        /// <summary>
        /// 取得會員資訊與訂單紀錄
        /// </summary>
        /// <returns>Customer:會員資訊，Orderheaders:訂單紀錄集合</returns>
        [HttpGet]
        [AuthFilter]
        public IActionResult GetMemberInfo()
        {
            string userid = HttpContext.Session.GetString("userid")!;

            //會員資訊
            var customer = from c in _context.Customers
                           where c.UserId == userid
                           select new
                           {
                               c.Name,
                               c.Phone,
                               c.Email,
                               c.ReceiverAddress
                           };

            //訂單紀錄

            var orderheaders = (from o in _context.Orderheaders
                                join c in _context.Customers on o.CustomerId equals c.CustomerId
                                join a in _context.Admlookups on o.Status equals a.Lookupid into statusLookup
                                from status in statusLookup
                                    // ****************************** 這邊先血死
                                where c.UserId == userid
                                select new
                                {
                                    o.OrderId,
                                    OrderDate = o.OrderDate.ToString()!.Substring(0, 10),
                                    o.Total,
                                    StatusName = status.Name
                                }).ToList();

            return Json(new { Customer = customer!, orderheaders });
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns>登出成功</returns>
        [HttpGet]
        [AuthFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return new ContentResult() { Content = "登出成功" };
        }

        /// <summary>
        /// 更新會員資訊
        /// </summary>
        /// <param name="userid"></param> 帳號
        /// <param name="name"></param> 姓名
        /// <param name="phone"></param> 電話
        /// <param name="email"></param> 信箱
        /// <param name="address"></param> 地址
        /// <returns>更新成功</returns>
        [HttpPost]
        [AuthFilter]
        public IActionResult UpdateMemberInfo(string userid, string name, string phone, string email, string address)
        {
            string sessionid = HttpContext.Session.GetString("userid")!;
            if (userid == sessionid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var member = _context.Customers.SingleOrDefault(m => m.UserId == userid);
                        if (member != null)
                        {
                            member.Name = name;
                            member.Phone = phone;
                            member.Email = email;
                            member.ReceiverAddress = address;
                        }
                        _context.SaveChanges();
                        transaction.Commit();
                        return new ContentResult() { Content = "更新成功" };
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                return new ContentResult() { Content = "帳號不正確" };
            }
        }
        /// <summary>
        /// 更新密碼
        /// </summary>
        /// <param name="userid"></param> 帳號
        /// <param name="o_password"></param> 舊密碼雜湊值
        /// <param name="n_password"></param> 新密碼雜湊值
        /// <returns>0:修改成功，1:舊密碼不正確</returns>
        [HttpPost]
        [AuthFilter]
        public IActionResult UpdatePassword(string userid, string o_password, string n_password)
        {
            string sessionid = HttpContext.Session.GetString("userid")!;
            Console.WriteLine($"userid: '{userid}'");
            Console.WriteLine($"sessionid: '{sessionid}'");
            Console.WriteLine(o_password);


            if (userid == sessionid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var member = _context.Customers.SingleOrDefault(m => m.UserId == userid);
                        var hash_o_password = HashPassword(o_password);
                        if (member != null && member.Password == hash_o_password)
                        {
                            var hash_n_password = HashPassword(n_password);
                            member.Password = hash_n_password;
                            _context.SaveChanges();
                            transaction.Commit();
                            return new ContentResult() { Content = "0" };
                        }
                        else
                        {
                            return new ContentResult() { Content = "1" };
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                return new ContentResult() { Content = "帳號不正確" };
            }
        }

        /// <summary>
        /// 查詢訂單明細
        /// </summary>
        /// <param name="orderid"></param> 訂單編號
        /// <returns>orderdetails:訂單明細集合，Total:總計</returns>
        [HttpPost]
        [AuthFilter]
        public IActionResult GetOrderDetail(string orderid)
        {
            //訂單明細集合
            var orderdetails = (from p in _context.Products
                                join od in _context.Orderdetails on p.ProductId equals od.ProductId
                                join oh in _context.Orderheaders on od.OrderId equals oh.OrderId
                                where oh.OrderId == orderid
                                select new
                                {
                                    p.Img,
                                    p.ProductName,
                                    p.Price,
                                    od.Qty,
                                    oh.OrderDate,
                                    od.Totle
                                }).ToList();

            //總計
            var total = _context.Orderheaders.Where(o => o.OrderId == orderid).Select(t => t.Total);
            Console.WriteLine("Order Details: " + orderdetails.Count);  // 查看有多少筆資料
            return Json(new { orderdetails, Total = total });
        }
    }

    /// <summary>
    /// 登入檢查
    /// </summary>
    public class AuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetString("userid") == null)
            {
                //合併後改成跳轉至登入頁面
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }
    }
}
