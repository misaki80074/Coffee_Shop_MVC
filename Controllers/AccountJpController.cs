﻿using Microsoft.AspNetCore.Mvc;

namespace Coffee.Controllers
{
	public class AccountJpController : Controller
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
	}
}
