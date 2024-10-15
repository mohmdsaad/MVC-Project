using System.ComponentModel.DataAnnotations;

namespace C42G02Demo.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "New Password Is Required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        
        [Required(ErrorMessage = "Confirm New Password Is Required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage = "Password Doesn't Match")]
        public string ConfirmPassword { get; set; }
    }
}
