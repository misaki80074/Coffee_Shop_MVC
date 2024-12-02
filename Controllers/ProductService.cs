using Coffee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Coffee.Controllers
{
	public class ProductService
	{
		private readonly ProjectContext _context;


		public ProductService(ProjectContext context)
		{
			_context = context;
		}
		// -----------------  獲取單個商品的資料
		public async Task<Product?> Get_Product_By_IdAsync(int id)
		{
			return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
		}
		//----------------- 拿到全部的 產地
		public async Task<List<string>> Get_All_Country_Async()
		{
			// 資料庫先抓
			var country = await _context.Products
				.Select(c => c.Country)
				.ToListAsync();

			// 在去空格+去除重複的國家名稱
			var all_country = country
				.SelectMany(c => c!.Split(','))     // 拆分每個 國家
				.Select(c => c.Trim())              // 去除每個 國家 名稱前後的空白
				.Distinct()                         // 去除重複的 國家
				.ToList();
			return all_country;
		}
		//----------------- 拿到全部的 烘焙程度
		public async Task<List<string>> Get_All_Baking_Async()
		{
			var all_baking = await _context.Products
				.Select(b => b.Baking)
				.Distinct()
				.ToListAsync();
			return all_baking!;
		}
		//----------------- 拿到全部的 風味
		public async Task<List<string>> Get_All_flavor_Async()
		{
			var flavor = await _context.Products
				.Select(f => f.Flavor)
				.ToListAsync();

			// 去空格+去重複的風味名稱
			var all_flavor = flavor
				.SelectMany(f => f!.Split(','))     // 拆分每個 風味
				.Select(c => c.Trim())              // 去除每個 風味 名稱前後的空白
				.Distinct()                         // 去除重複的 風味
				.ToList();
			return all_flavor;
		}
		//----------------- 拿到全部的 處理法
		public async Task<List<string>> Get_All_Method_Async()
		{
			var all_method = await _context.Products
				.Select(b => b.Method)
				.Distinct()
				.ToListAsync();
			return all_method!;
		}
		//----------------- 拿到全部的 重量
		public async Task<List<string>> Get_All_Weight_Async()
		{
			var all_method = await _context.Products
				.Select(b => b.Weight)
				.Distinct()
				.ToListAsync();
			return all_method!;
		}
		//----------------- 拿到全部的 保存期限
		public async Task<List<string>> Get_All_Timelimit_Async()
		{
			var all_timelimit = await _context.Products
				.Select(t => t.Timelimit)
				.Distinct()
				.ToListAsync();
			return all_timelimit!;
		}

		// 練習寫泛型整合起來
		//----------------- 拿到產品 香氣
		public async Task<int> Get_Fragrance_Async(int id)
		{
			var fragrance = await _context.Products
				.Where(c => c.Id == id)       // 篩選條件
				.Select(b => b.Fragrance)     // 選擇 Fragrance
				.FirstOrDefaultAsync();       // 確保返回單一結果
			return (int)fragrance;
		}
		//----------------- 拿到產品 酸味
		public async Task<int> Get_Sour_Async(int id)
		{
			var sour = await _context.Products
				.Where(c => c.Id == id)
				.Select(b => b.Sour)
				.FirstOrDefaultAsync();
			return (int)sour;
		}
		//----------------- 拿到產品 苦味
		public async Task<int> Get_Bitter_Async(int id)
		{
			var bitter = await _context.Products
				.Where(c => c.Id == id)
				.Select(b => b.Bitter)
				.FirstOrDefaultAsync();
			return (int)bitter;
		}
		//----------------- 拿到產品 甜味
		public async Task<int> Get_Sweet_Async(int id)
		{
			var sweet = await _context.Products
				.Where(c => c.Id == id)
				.Select(b => b.Sweet)
				.FirstOrDefaultAsync();
			return (int)sweet;
		}
		//----------------- 拿到產品 厚實
		public async Task<int> Get_Strong_Async(int id)
		{
			var strong = await _context.Products
				.Where(c => c.Id == id)
				.Select(b => b.Strong)
				.FirstOrDefaultAsync();
			return (int)strong;
		}


	}
}
