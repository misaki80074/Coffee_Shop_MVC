using Microsoft.AspNetCore.Mvc;
using Coffee.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Member.Controllers;

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
}