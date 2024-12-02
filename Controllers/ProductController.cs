using Coffee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProjectContext _context;

        public ProductController(ProjectContext context)
        {
            _context = context;
        }
       
        public async Task<IActionResult> Index()
        {
            ViewBag.Demo = await _context.Products.ToListAsync();
            return View();
        }
    }
}
