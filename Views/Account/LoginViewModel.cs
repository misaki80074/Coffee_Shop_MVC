using System.ComponentModel.DataAnnotations;

namespace Coffee.Views.Account
{
    public class LoginViewModel
    {
		[Required]
		public string? UserId { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[StringLength(12, ErrorMessage = "密碼長度至少為 {2} 個字符", MinimumLength = 8)]
		[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)[A-Za-z\d]{8,}$",
		ErrorMessage = "密碼必須包含大小寫、數字")]
		public string? Password { get; set; }
	}
}
