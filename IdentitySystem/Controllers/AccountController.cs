using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentitySystem.Models;
using IdentitySystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentitySystem.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> sigInManger;
        private readonly UserManager<AppUser> userManager;
        public AccountController(SignInManager<AppUser> sigInManger, UserManager<AppUser> userManager)
        {
            this.sigInManger = sigInManger;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                // 1 : Copy Data from RegisterViewModel to IdentityUser
                AppUser user = new AppUser
                {
                    UserName = userModel.Email,
                    Email = userModel.Email,
                    Country = userModel.Country
                };
                // 2 : store user in DB :UserManager class
                var result = await userManager.CreateAsync(user, userModel.Password);
                //3 : Process ? Successed or Fail
                if (result.Succeeded)
                {
                    await sigInManger.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");

                }
                // 4 : in case of any error in registerations
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await sigInManger.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel userModel , string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await sigInManger.PasswordSignInAsync(userModel.Email, userModel.Password,userModel.RememberMe , false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                    else
                        return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid Email / Password");
            }
            return View(userModel);
        }
        [AcceptVerbs("Get" , "Post")]
        public async Task<IActionResult> IsEmailExist(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return Json(true);
            else return Json($"The Email {email} is Already in Use");
        }
    }
}