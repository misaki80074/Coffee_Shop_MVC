using Microsoft.AspNetCore.Mvc;
using Coffee.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;

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
    }
    public class AuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 檢查是否有登入
            if (filterContext.HttpContext.Session.GetString("userid") == null)
            {
                // 未登入，重新定向至登入頁面
                filterContext.Result = new RedirectResult("Login");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}