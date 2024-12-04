using Microsoft.AspNetCore.Mvc;

namespace Coffee.Controllers
{
	public class DetailController : Controller
	{
		private readonly ProductService _productService;

		public DetailController(ProductService productService)
		{
			_productService = productService;
		}

		public async Task<IActionResult> Detail(int id)
		{

			// 使用 ProductService 獲取商品資料
			var product = await _productService.Get_Product_By_IdAsync(id);
			if (product == null)
			{
				return NotFound();
			}

			// 獲取欄位不重複值
			var all_country = await _productService.Get_All_Country_Async();
			var all_baking = await _productService.Get_All_Baking_Async();
			var all_flavor = await _productService.Get_All_flavor_Async();
			var all_method = await _productService.Get_All_Method_Async();
			var all_weight = await _productService.Get_All_Weight_Async();
			var all_time_limit = await _productService.Get_All_Timelimit_Async();

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
			var fragrance = await _productService.Get_Fragrance_Async(id);
			// 酸度
			var sour = await _productService.Get_Sour_Async(id);
			// 苦味
			var bitter = await _productService.Get_Bitter_Async(id);
			// 苦味
			var sweet = await _productService.Get_Sweet_Async(id);
			// 厚實
			var strong = await _productService.Get_Strong_Async(id);
			return Json(new { fragrance = fragrance, sour = sour, bitter = bitter, sweet = sweet, strong = strong });
		}

		// 研磨粗細 萃取係數 注水方式的 API
		public JsonResult Grinding_Thickness_Extraction_Coefficient_Wate_Injection_Method_Api(string baking_level)
		{

			string grind_size;
			string extraction_conditions;
			string add_water;

			switch (baking_level)
			{
				case "淺焙":
					grind_size = "建議採用精細研磨，顆粒細緻如味精大小。此研磨粗細能讓咖啡粉充分展現淺焙咖啡特有的香氣與細膩的風味特質。";
					extraction_conditions = "通常需要較長的萃取時間，因為需要更多時間來萃取酸味和香氣";
					add_water = "建議三段注水：首先注水悶蒸，讓咖啡粉充分濕潤。之後按均勻的節奏分兩次注水，每次讓水位慢慢下降再注入新水";
					break;
				case "中淺焙":
					grind_size = "建議採用細研磨，顆粒稍粗於精細研磨但仍保持細緻。此級別的研磨更能突顯中淺焙咖啡的均衡香氣與明亮風味。";
					extraction_conditions = "萃取時間比淺焙略短，能保持均衡的酸甜感";
					add_water = "建議三段注水：注水悶蒸完，然後以均勻速度注入第二段水，當水位下降至粉層表面時，進行第三段注水，保持水流輕柔，避免過度攪動粉層";
					break;
				case "中焙":
					grind_size = "建議採用中細研磨，顆粒大小適中，類似海鹽顆粒。這種研磨粗細可展現中焙咖啡香氣的豐富層次，並保有良好的平衡感。";
					extraction_conditions = "萃取時間適中，可以同時平衡酸味和苦味";
					add_water = "建議兩段注水：先進行悶蒸，之後以均勻速度將水分兩段注入，可稍微提高水位來增加萃取率，但也要避免過多攪動粉層";
					break;
				case "中深焙":
					grind_size = "建議採用中等研磨，顆粒稍粗，具有穩定均勻的粗細比例。此研磨粗細能有效傳達中深焙咖啡豆的醇厚特性與溫潤口感。";
					extraction_conditions = "萃取時間不宜過長，以保留其甜感和微微的苦味";
					add_water = "建議兩段注水：悶蒸後開始均勻注水，第二段注水量略少於第一段，以達到合適的濃度。注意保持水流穩定，以避免過度萃取苦味";
					break;
				case "深焙":
					grind_size = "建議採用粗研磨，顆粒較粗，類似芝麻顆粒大小。粗研磨適合更好地展現深焙咖啡豆的濃郁風味及其香醇的餘韻。";
					extraction_conditions = "通常不需要太長的萃取時間，以避免過度苦澀";
					add_water = "建議單段注水：悶蒸完成後，緩慢均勻地注入所有水量，保持水流細而穩定。避免多段注水，以減少苦澀感並保持順滑的口感。可以稍微加快注水速度，注意不要過度萃取";
					break;
				default:
					grind_size = "錯誤錯誤";
					extraction_conditions = "錯誤錯誤";
					add_water = "錯誤錯誤";
					break;
			}
			// 返回JSON格式的研磨粗細
			return Json(new { grindsize = grind_size, extractionconditions = extraction_conditions, addwater = add_water });
		}
	}
}
