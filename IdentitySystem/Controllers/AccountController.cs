using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentitySystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentitySystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> sigInManger;
        private readonly UserManager<IdentityUser> userManager;
        public AccountController(SignInManager<IdentityUser> sigInManger, UserManager<IdentityUser> userManager)
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
                IdentityUser user = new IdentityUser
                {
                    UserName = userModel.Email,
                    Email = userModel.Email,
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
        public async Task<IActionResult> Login(LoginViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await sigInManger.PasswordSignInAsync(userModel.Email, userModel.Password,userModel.RememberMe , false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid Email / Password");
            }
            return View(userModel);
        }
    }
}