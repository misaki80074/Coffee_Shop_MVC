using Microsoft.AspNetCore.Mvc;

namespace Coffee.Controllers
{
	public class DetailJpController : Controller
	{
		private readonly ProductServiceJp _productServiceJp;

		public DetailJpController(ProductServiceJp productServiceJp)
		{
			_productServiceJp = productServiceJp;
		}

		public async Task<IActionResult> DetailJp(int id)
		{

			// 使用 ProductService 獲取商品資料
			var product = await _productServiceJp.Get_Product_By_IdAsync(id);
			if (product == null)
			{
				return NotFound();
			}

			// 獲取欄位不重複值
			var all_country = await _productServiceJp.Get_All_Country_Async();
			var all_baking = await _productServiceJp.Get_All_Baking_Async();
			var all_flavor = await _productServiceJp.Get_All_flavor_Async();
			var all_method = await _productServiceJp.Get_All_Method_Async();
			var all_weight = await _productServiceJp.Get_All_Weight_Async();
			var all_time_limit = await _productServiceJp.Get_All_Timelimit_Async();

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
			var fragrance = await _productServiceJp.Get_Fragrance_Async(id);
			// 酸度
			var sour = await _productServiceJp.Get_Sour_Async(id);
			// 苦味
			var bitter = await _productServiceJp.Get_Bitter_Async(id);
			// 苦味
			var sweet = await _productServiceJp.Get_Sweet_Async(id);
			// 厚實
			var strong = await _productServiceJp.Get_Strong_Async(id);
			return Json(new { fragrance = fragrance, sour = sour, bitter = bitter, sweet = sweet, strong = strong });
		}

		// 研磨粗細 萃取係數 注水方式的 API
		public JsonResult Grinding_Thickness_Extraction_Coefficient_Wate_Injection_Method_Api_Jp(string baking_level)
		{

			string grind_size;
			string extraction_conditions;
			string add_water;

			switch (baking_level)
			{
				case "ライト":
					grind_size = "極細挽きは推奨です。これにより、ライトローストコーヒー特有の繊細な香りや風味を最大限に引き出すことができます。";
					extraction_conditions = "豆本来の風味を最大限に引き出すために、ゆっくりと時間をかけて抽出するのがおすすめです。";
					add_water = "三段階の注水をおすすめします。最初に蒸らしを行い、コーヒー粉を十分に湿らせます。その後、二回に分けてゆっくりと注水し、コーヒー粉と水を十分に接触させます。";
					break;
				case "シナモン":
					grind_size = "極細挽きより少し粗めの細挽きがおすすめです。これにより、中浅煎りコーヒーの豊かな香りとあっさりな味わいを最大限に引き出すことができます。";
					extraction_conditions = "ライトローストより短い抽出時間で、酸味と甘みのバランスを引き出すのができます。";
					add_water = "三段階の注水をおすすめします。蒸らしの後、均一な速度で二回目の注水を行い、水位がコーヒー粉の表面に達したら、優しく水を注ぎます。コーヒー粉を攪拌しないように。";
					break;
				case "ミディアム":
					grind_size = "海塩のような中細挽きが最適です。これにより、ミディアムローストコーヒーの複雑な香りと、酸味と苦味のバランスの風味を楽しむのができます。";
					extraction_conditions = "適切な抽出時間で、酸味と苦味のバランスを引き出すのができます。";
					add_water = "二段階の注水を推奨します。蒸らしの後、均一な速度で二回に分けて注水します。抽出率を高めるために、水位を少し高くしても良いですが、コーヒー粉を攪拌しすぎないように注意してください。";
					break;
				case "シティ":
					grind_size = "海塩より少し粗めの中挽きが最適です。これにより、シティローストコーヒーの複雑な風味と、重厚感のある味わいを最大限に引き出すことができます。";
					extraction_conditions = "甘みとほろ苦さのバランスを保つために、抽出時間を短めに抑えることが大切です。";
					add_water = "二段階の注水を推奨します。蒸らしの後、均一な速度で注水し、二回目の注水量は1回目よりも少し少なめにすることで、最適な濃度を得ることができます。水流を安定させることで、苦味が過度に抽出されるのを防ぎます。";
					break;
				case "フレンチ":
					grind_size = "ゴマのような粗挽きが最適です。フレンチローストコーヒー豆の濃厚な風味と豊かなコクを引き出すことができます。";
					extraction_conditions = "苦味を抑えるために、抽出時間を短めに設定することが大切です。";
					add_water = "一度の注水を推奨します。蒸らしの後、ゆっくりと均一なスピードで全量の水を注ぎます。水流を細く安定させることで、苦味を抑え、滑らかな口当たりを実現できます。複数回に分けて注ぐと、苦味が強くなる可能性があるため、一度に注ぐことをおすすめします。";
					break;
				default:
					grind_size = "エラー";
					extraction_conditions = "エラー";
					add_water = "エラー";
					break;
			}
			// 返回JSON格式的研磨粗細
			return Json(new { grindsize = grind_size, extractionconditions = extraction_conditions, addwater = add_water });
		}
	}
}
