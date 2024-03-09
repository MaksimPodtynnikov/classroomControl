using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using classroomLibrary.Data.Interfaces;
using classroomLibrary.ViewModels.Account;
using classroomLibrary.Services.Interfaces;

namespace classroomLibrary.Controllers
{
    public class AccountController : Controller
    {
        private readonly IWorkersService _accountService;

        public AccountController(IWorkersService accountService)
        {
            _accountService = accountService;
        }


        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.Login(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        /*[HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.ChangePassword(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return Json(new { description = response.Description });
                }
            }
            var modelError = ModelState.Values.SelectMany(v => v.Errors);

            return StatusCode(StatusCodes.Status500InternalServerError, new { modelError.FirstOrDefault().ErrorMessage });
        }*/
    }
}
