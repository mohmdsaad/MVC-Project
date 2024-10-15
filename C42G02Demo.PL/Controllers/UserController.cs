using AutoMapper;
using C42G02Demo.DAL.Model;
using C42G02Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C42G02Demo.PL.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager , 
							 IMapper mapper)
        {
			_userManager = userManager;
            _mapper = mapper;
        }

		public async Task<IActionResult> Index(string SearchValue)
		{
			if (string.IsNullOrEmpty(SearchValue))
			{
				var Users = await _userManager.Users.Select(
					U => new UserViewModel()
					{
						Id = U.Id,
						FName = U.FName,
						LName = U.LName,
						Email = U.Email,
						PhoneNumber = U.PhoneNumber,
						Roles = _userManager.GetRolesAsync(U).Result
					}).ToListAsync();
				return View(Users);
			}
			else
			{
				var User = await _userManager.FindByEmailAsync(SearchValue);

				var MappedUser = new UserViewModel()
				{
					Id = User.Id,
					FName = User.FName,
					LName = User.LName,
					Email = User.Email,
					PhoneNumber = User.PhoneNumber,
					Roles = _userManager.GetRolesAsync(User).Result
				};
				return View(new List<UserViewModel> { MappedUser });
			}
		}

		[HttpGet]
		public async Task<IActionResult> Details(string id , string viewName = "Details")
		{
			if(id is null)
				return BadRequest();

			var user = await _userManager.FindByIdAsync(id);

			if(user is null)
				return NotFound();

			var mappedUser = _mapper.Map<ApplicationUser, UserViewModel>(user);

			return View(viewName, mappedUser);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			return await Details(id, "Edit");
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel model)
		{
			if (id != model.Id)
				return BadRequest();

			if (ModelState.IsValid)
			{
				try
				{
                    var User = await _userManager.FindByIdAsync(model.Id);
					
					User.PhoneNumber = model.PhoneNumber;
					User.FName = model.FName;
					User.LName = model.LName;

                    await _userManager.UpdateAsync(User);

                    return RedirectToAction(nameof(Index));
				}
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
			
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(string id) 
		{
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete([FromRoute] string id)
		{
            try
            {
				var User = await _userManager.FindByIdAsync(id);
					
				await _userManager.DeleteAsync(User);

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
                ModelState.AddModelError(string.Empty, ex.Message);

				return RedirectToAction("Error", "Home");
            }

		}
    }
}
