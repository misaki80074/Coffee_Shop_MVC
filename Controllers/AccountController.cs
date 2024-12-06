using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
// ----------------------------------- 新增 -------------------------------- //
using System.Security.Cryptography;
// ------------------------------------------------------------------------ //

namespace Coffee.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Member()
        {
            return View();
        }

        // ------------------------------------------------------- 用來處理密碼雜湊的方法 --------------------------------------------- // 
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
        // ---------------------------------------------------------------------------------------------------------------------------- // 

        // ------------------------------------------------------- 註冊頁面-檢查帳號是否存在的方法 POST --------------------------------------------- // 
        [HttpPost]
        public IActionResult CheckUserid([FromBody] string userid)
        {
            // ( 假帳號 )
            var UserIds = new List<string> { "testuser", "admin", "user123" };

            // 判斷帳號是否存在
            var HasUserIds = UserIds.Contains(userid);

            if (HasUserIds)
            {
                return Json(new { status = "0" });   // 存在
            }
            return Json(new { status = "1" });      //  不存在
        }
        // ----------------------------------------------------------------------------------------------------------------------------------------- // 

        // ------------------------------------------------------- 註冊頁面-註冊按鈕 POST --------------------------------------------- // 
        [HttpPost]
        public IActionResult Register([FromForm] string userId, [FromForm] string password, [FromForm] string name, [FromForm] string phone, [FromForm] string email)
        {       
            // 密碼雜湊處理
            var hashedPassword = HashPassword(password); // HashPassword 會使用後端安全的加密方法

            // 可進行資料處理（存入資料庫，這裡暫時不做）

            // 註冊成功
            return Json(new { status = "1" });
        }
        // ---------------------------------------------------------------------------------------------------------------------------- // 

        // ------------------------------------------------------- 登入頁面-登入按鈕 POST --------------------------------------------- // 
        [HttpPost]
        public IActionResult Login([FromForm] string userId, [FromForm] string password)
        {
           
            // ( userId == 資料庫任一筆帳號 )
            if (userId == "w")
            {
                return Json(new { status = "1" });   // 未註冊
            }

            // 密碼雜湊處理
            var hashedPassword = HashPassword(password);

            // 比對雜湊後的密碼 ( hashedPassword = 資料庫密碼 )
            if (hashedPassword == HashPassword(password) + "6666666666")  
            {
                return Json(new { status = "0" }); // 登入成功
            }
            else 
            {
                return Json(new { status = "2" }); // 密碼錯誤
            }
        }
        // ----------------------------------------------------------------------------------------------------------------------------- // 
    }
}



