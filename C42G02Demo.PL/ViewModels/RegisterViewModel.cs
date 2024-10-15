using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace C42G02Demo.PL.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Fisrt Name Is Required")]
		public string FName { get; set; }
		
		[Required(ErrorMessage = "Last Name Is Required")]
		public string LName { get; set; }
		
		[Required(ErrorMessage = "Email Is Required")]
		[EmailAddress(ErrorMessage ="Email Is Invalid")]
		public string Email { get; set; }
		
		[Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		
		[Required(ErrorMessage = "Confirm Password Is Required")]
		[DataType(DataType.Password)]
		[Compare("Password" , ErrorMessage ="Passwords Doesn't Match")]
		public string ConfirmPassword { get; set; }
		
		//[Required(ErrorMessage = "You must agree on our terms!")]
		public bool IsAgree { get; set; }
	}
}
