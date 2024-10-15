using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace C42G02Demo.DAL.Model
{
	public class ApplicationUser : IdentityUser
	{
        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }
        [Required]
        public bool IsAgree { get; set; }
    }
}
