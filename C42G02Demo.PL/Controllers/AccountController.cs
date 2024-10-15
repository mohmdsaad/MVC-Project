using C42G02Demo.DAL.Model;
using C42G02Demo.PL.Helpers;
using C42G02Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace C42G02Demo.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}
        // Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) // Server side validation
            {
                var User = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    FName = model.FName,
                    LName = model.LName,
                    Email = model.Email,
                    IsAgree = model.IsAgree
                };

                var Result = await _userManager.CreateAsync(User, model.Password);

                if (Result.Succeeded)
                    RedirectToAction(nameof(Login));
                else
                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        // Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);

                if(User is not null)
                {
                    bool CheckedPassword = await _userManager.CheckPasswordAsync(User, model.Password);

                    if (CheckedPassword)
                    {
                        //Login
                        var Result = await _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false);

                        if (Result.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
					}
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Incorrect Password!");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email not exists !");
                }
            }
            return View(model);
        }

        // Sign out
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        // Forget Password
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);

                if (User is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(User);
                    // https://localhost:44348/Account/ResetPassword?email=mohamedsaadiio345@gmail.com
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = User.Email , Token = token}, Request.Scheme);

                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = "ResetPasswordLink"
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourIndex));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email Doesn't Exist");
                }

            }

            return View("ForgetPassword", model);

        }

        public IActionResult CheckYourIndex()
        {
            return View();
        }

		// Reset Password
        public IActionResult ResetPassword(string email , string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
				string email = TempData["email"] as string;
				string token = TempData["token"] as string;

				var User = await _userManager.FindByEmailAsync(email);
				var Result =  await _userManager.ResetPasswordAsync(User, token, model.NewPassword);
                
                if (Result.Succeeded)
                    return RedirectToAction(nameof(Login));
                else
                    foreach(var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
			}
            return View(model);
        }
	}
}
