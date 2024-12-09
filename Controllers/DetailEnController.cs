using Microsoft.AspNetCore.Mvc;

namespace Coffee.Controllers
{
	public class DetailEnController : Controller
	{
		private readonly ProductServiceEn _productServiceEn;

		public DetailEnController(ProductServiceEn productServiceEn)
		{
			_productServiceEn = productServiceEn;
		}

		public async Task<IActionResult> DetailEn(int id)
		{

			// 使用 ProductService 獲取商品資料
			var product = await _productServiceEn.Get_Product_By_IdAsync(id);
			if (product == null)
			{
				return NotFound();
			}

			// 獲取欄位不重複值
			var all_country = await _productServiceEn.Get_All_Country_Async();
			var all_baking = await _productServiceEn.Get_All_Baking_Async();
			var all_flavor = await _productServiceEn.Get_All_flavor_Async();
			var all_method = await _productServiceEn.Get_All_Method_Async();
			var all_weight = await _productServiceEn.Get_All_Weight_Async();
			var all_time_limit = await _productServiceEn.Get_All_Timelimit_Async();

			// 傳遞資料到 View
			ViewBag.all_country = all_country;
			ViewBag.all_baking = all_baking;
			ViewBag.all_flavor = all_flavor;
			ViewBag.all_method = all_method;
			ViewBag.all_weight = all_weight;
			ViewBag.all_time_limit = all_time_limit;

			// 傳資料到前端 view 給 JS 抓 url 的 id 值
			ViewBag.shareid = id;
			return View(product);
		}

		// 味道分布的 API 
		public async Task<JsonResult> Taste_Distribution_Api(int id)
		{
			// 香氣
			var fragrance = await _productServiceEn.Get_Fragrance_Async(id);
			// 酸度
			var sour = await _productServiceEn.Get_Sour_Async(id);
			// 苦味
			var bitter = await _productServiceEn.Get_Bitter_Async(id);
			// 苦味
			var sweet = await _productServiceEn.Get_Sweet_Async(id);
			// 厚實
			var strong = await _productServiceEn.Get_Strong_Async(id);
			return Json(new { fragrance = fragrance, sour = sour, bitter = bitter, sweet = sweet, strong = strong });
		}

		// 研磨粗細 萃取係數 注水方式的 API
		public JsonResult Grinding_Thickness_Extraction_Coefficient_Wate_Injection_Method_Api_En(string baking_level)
		{

			string grind_size;
			string extraction_conditions;
			string add_water;

			switch (baking_level)
			{
				case "Light":
					grind_size = "For light roasting coffee, fine grind, as fine as MSG, is ideal to bring out the subtle nuances of flavor and aroma.";
					extraction_conditions = "A longer extraction time is needed for light roasting coffee to bring out its bright acidity and delicate flavors.";
					add_water = "Pour water in three stages. Begin with a bloom, then pour evenly in two stages, allowing the water level to drop slightly before each subsequent pour.";
					break;
				case "Cinnamon":
					grind_size = "We suggest using a medium-fine grind, slightly coarser than a fine grind, to best showcase the balanced and bright characteristics of cinnamon roasting.";
					extraction_conditions = "A shorter extraction time is usually needed for cinnamon roasting to achieve a balanced profile of acidity and sweetness.";
					add_water = "We suggest a three-pour method: bloom, then pour to the top, and finally, a gentle pour once the water level drops.";
					break;
				case "Medium":
					grind_size = "For medium roasting, medium-fine grind is ideal to bring out the complex and balanced flavor profile.";
					extraction_conditions = "Medium roast requires a moderate extraction time to achieve a balance between acidity and bitterness.";
					add_water = "Two-pour method is recommended: begin with a bloom, followed by two even pours. Slightly raising the water level between pours can increase extraction, but avoid disturbing the coffee bed.";
					break;
				case "City":
					grind_size = "Medium grind, slightly coarse with a consistent particle size, is recommended. This grind size will help to highlight the rich body and smooth flavor of city roasting.";
					extraction_conditions = "The extraction time for city roasting should be carefully controlled to preserve the sweetness and subtle bitterness.";
					add_water = "We suggest two-pour method: bloom, then pour evenly, followed by a slightly smaller second pour to achieve the desired concentration. Steady pour will help prevent over-extraction.";
					break;
				case "French":
					grind_size = "Coarse grind, similar in size to sesame seeds, is recommended. This grind size will better highlight the rich flavor and smooth aftertaste of french roasting.";
					extraction_conditions = "To prevent excessive bitterness, it's best to avoid over-extracting french roasting coffee.";
					add_water = "Single pour method is recommended to avoid over-extraction and bitterness. After blooming, pour all the water slowly and evenly.";
					break;
				default:
					grind_size = "Error";
					extraction_conditions = "Error";
					add_water = "Error";
					break;
			}
			// 返回JSON格式的研磨粗細
			return Json(new { grindsize = grind_size, extractionconditions = extraction_conditions, addwater = add_water });
		}
	}
}
