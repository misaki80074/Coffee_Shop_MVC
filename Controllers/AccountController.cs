using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Coffee.Models;
using System.IO;

using Microsoft.CodeAnalysis;

using System.Linq;
using System.Net.NetworkInformation;
using Coffee.Views.Account;

namespace Coffee.Controllers
{
    public class AccountController : Controller
    {
        private readonly ProjectContext _context;

        public AccountController(ProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var userId = _context.Customers.FirstOrDefault(u => u.UserId == model.UserId);
                if (userId != null)
                {
                    var login = _context.Customers.FirstOrDefault(u => u.UserId == model.UserId && u.Password == model.Password);
                    if (login != null)
                    {
                        // 登入成功，設定 Cookie 或 Session
                        HttpContext.Session.SetString("loginId", login.CustomerId);
                        TempData["id2"] = login.CustomerId;
                        return RedirectToAction("Index", "Account"); //登入後預設為Account的Index畫面
                    }
                }
                else { return View("Register"); }
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("loginId");
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Index()
        {
			var loginId = HttpContext.Session.GetString("loginId");
			if (loginId == null)
			{
				return RedirectToAction("Login", "Account");
			}
			TempData["id2"] = loginId;

			//大頭照
			var avatar = await _context.Customers
								  .FirstOrDefaultAsync(c => c.CustomerId == loginId);
			if (avatar != null && avatar.ImgSrc != null)
			{
				TempData["avatar2"] = avatar.ImgSrc;
			}
			else
			{
				// 若 avatar 或 ImgSrc 為 null，您可以設置一個預設值，或進行錯誤處理
				TempData["avatar2"] = "/img/Member/defaultAvatar.jpg"; // 設置預設圖片
			}

			// 查詢Orderheader資料表
			var orderHeaders = await _context.Orderheaders.Where(o => o.CustomerId == loginId).ToListAsync();
			// 為每個OrderHeader準備HeaderViewModel
			var headerViewModels = new List<HeaderViewModel>();

			foreach (var o in orderHeaders)
			{
				var headerViewModel = new HeaderViewModel
				{
					OrderId = o.OrderId,
					CustomerId = o.CustomerId,
					OrderDate = o.OrderDate,
					Payment = o.Payment,
					Total = o.Total,
					OrderStatus = o.Status,
					ShipStatus = o.ShipStatus
				};
				// 查詢對應OrderId的OrderDetail資料
				var orderDetails = await _context.Orderdetails
									   .Where(od => od.OrderId == o.OrderId)
									   .ToListAsync();

				// 填充OrderDetails的產品名稱
				foreach (var d in orderDetails)
				{
					var product = await _context.Products
									  .FirstOrDefaultAsync(p => p.ProductId == d.ProductId);
					if (product != null)
					{
						headerViewModel.Orderdetails.Add(new DetailViewModel
						{
							ProductId = d.ProductId,
							ProductName = product.ProductName,
							Qty = d.Qty,
							UnitPrice = d.UnitPrice,
							Totle = d.Totle
						});
					}
				}
				headerViewModels.Add(headerViewModel);
			}
			return View(headerViewModels);
		}

        public async Task<IActionResult> Details(string? id)
        {
			var loginId = HttpContext.Session.GetString("loginId");
			TempData["id2"] = loginId;

			if (id == null)
			{
				return NotFound();
			}

			//大頭照
			var avatar = await _context.Customers
								  .FirstOrDefaultAsync(c => c.CustomerId == loginId);
			if (avatar != null && avatar.ImgSrc != null)
			{
				TempData["avatar2"] = avatar.ImgSrc;
			}
			else
			{
				// 若 avatar 或 ImgSrc 為 null，您可以設置一個預設值，或進行錯誤處理
				TempData["avatar2"] = "/img/Member/defaultAvatar.jpg"; // 設置預設圖片
			}


			//找OrderId
			var orderHeader = await _context.Orderheaders
							   .FirstOrDefaultAsync(o => o.OrderId == id);

			if (orderHeader == null)
			{
				return NotFound();
			}

			// 查找該OrderId的所有Orderdetail資料
			var orderDetails = await _context.Orderdetails
								   .Where(od => od.OrderId == id)
								   .ToListAsync();

			// 構建ViewModel
			var detailViewModels = new List<DetailViewModel>();

			foreach (var d in orderDetails)
			{
				var product = await _context.Products
									  .FirstOrDefaultAsync(p => p.ProductId == d.ProductId);

				detailViewModels.Add(new DetailViewModel
				{
					ProductId = d.ProductId,
					ProductName = product?.ProductName,  // 確保ProductName是從Product資料表取得的
					Qty = d.Qty,
					Uom = d.Uom,
					UnitPrice = d.UnitPrice,
					Totle = d.Totle
				});
			}

			// 將OrderHeader與OrderDetails傳遞給視圖
			var viewModel = new HeaderViewModel
			{
				OrderId = orderHeader.OrderId,
				OrderDate = orderHeader.OrderDate,
				CustomerId = orderHeader.CustomerId,
				Total = orderHeader.Total,
				OrderStatus = orderHeader.Status,
				ShipStatus = orderHeader.ShipStatus,
				Orderdetails = detailViewModels
			};

			return View(viewModel);
		}

        public IActionResult Register()
        {
            return View();
        }

        private readonly string _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("CustomerId,UserId,Password,Email,Phone,Name,Gender,Birthday,Language,ReceiverAddress,CreateUser,UpdateUser,CreateDate,UpdateDate")] Customer customer, IFormFile? ImgSrc)
        {
            //if (ModelState.IsValid)
            //{
                // 檢查是否有檔案上傳
                if (ImgSrc != null)
                {
                    // 確保 上傳資料夾存在
                    if (!Directory.Exists(_uploadsFolder))
                    {
                        Directory.CreateDirectory(_uploadsFolder);
                    }

                    // 取得檔案名稱與副檔名
                    var fileName = Path.GetFileName(ImgSrc.FileName);
                    var filePath = Path.Combine(_uploadsFolder, fileName);

                    // 儲存檔案到伺服器
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImgSrc.CopyToAsync(stream);  // 儲存上傳檔案到指定路徑
                    }

                    // 將檔案的相對路徑儲存到 ImgSrc 屬性
                    customer.ImgSrc = "/images/" + fileName;
                }

                // 查詢資料庫中最大的 CustomerId
                var lastCustomerId = await _context.Customers
                    .OrderByDescending(c => c.CustomerId)  // 按照 CustomerId 進行降序排列
                    .FirstOrDefaultAsync();

                // 計算新的 CustomerId
                string newCustomerId;
                // 提取最後一個 CustomerId 的數字部分
                string lastId = lastCustomerId!.CustomerId.Substring(4);  // 假設 CustomerId 格式為 CUST00000001
                int lastNumber = int.Parse(lastId);  // 將數字部分轉換為整數
                int newNumber = lastNumber + 1;  // 新的數字是上一個數字 + 1
                newCustomerId = "CUST" + newNumber.ToString("D8");  // 生成新的 CustomerId，並保證八位數字，不足位補零
                
                // 檢查新的 CustomerId 是否已經存在
                while (await _context.Customers.AnyAsync(c => c.CustomerId == newCustomerId))
                {
                    newNumber++;
                    newCustomerId = "CUST" + newNumber.ToString("D8");
                }

                customer.CustomerId = newCustomerId;  // 設置新的 CustomerId

                customer.CreateDate = DateTime.Now;
                customer.UpdateDate = DateTime.Now;
                customer.CreateUser = customer.UserId;
                customer.UpdateUser = customer.UserId;

                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}

            //return View(customer);
    }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {

			if (id == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            //var customer = await _context.Customers.FirstOrDefaultAsync(u=>u.CustomerId==id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CustomerEditModel customerEditModel)
        {
            Customer customer = new Customer();

            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            var customerEditModels = from o in _context.Customers
                                     where o.CustomerId == customer.CustomerId
                                     select new CustomerEditModel
                                     {
                                         CustomerId = customer.CustomerId,

                                     };

            //if (ModelState.IsValid)
            //{
            try
            {
                //_context.Entry(customer).Property(x => x.Id).IsModified = false;
                customer.UpdateDate = DateTime.Now;
                customer.UpdateUser = customer.UserId;

                _context.Update(customer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.CustomerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            //}
            //return View(customer);
        }
        private bool CustomerExists(string id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }

        
    }
}
