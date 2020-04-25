using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebAppUsers.Models;

namespace WebAppUsers.Controllers
{
    public class AccauntController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private string accauntLogin;
        

        public string AccaunLogin{ get => accauntLogin; set => accauntLogin = value; }

        public AccauntController(Microsoft.AspNetCore.Identity.UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        

        }




        public async Task<IdentityResult> UpdateAccauntLastLoginDate(LoginViewModel model)
        {

            User user = await _userManager.FindByNameAsync(model.Name);
            user.LastLoginingDate = DateTime.Now;
            return await _userManager.UpdateAsync(user);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Name };
               

                var result = await _userManager.CreateAsync(user, model.Password);
               


                if (result.Succeeded)
                {

                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Login", "Accaunt");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);

        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, true);
                User checkuser = await _userManager.FindByNameAsync(model.Name);

              
              

              

                if (result.Succeeded)
                {
                    


                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {

                        return Redirect(model.ReturnUrl);

                    }
                    else
                    {
                        
                        await UpdateAccauntLastLoginDate(model);
                        AccaunLogin = model.Name;
                        return new RedirectResult("/Accaunt/UsersTable");



                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login or password");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        
        public IActionResult UsersTable()
        {
            var result = _userManager.FindByNameAsync(User.Identity.Name);

            if (User.Identity.IsAuthenticated && result.Result.LockoutEnabled == true)
            {
                return View("~/Views/Home/UsersTable.cshtml", _userManager.Users.ToList());
            }
           return new RedirectResult("/Accaunt/Login");

        }

        [HttpPost]
        public async Task<IActionResult> ChangeData(string[] formData)
        {
            if (formData.Length < 1) return Ok();
            if (formData[0] == "Blocked")
            {

                await BlockedUserAsync(formData);
                var result  = _userManager.FindByNameAsync(User.Identity.Name);
                if (Array.IndexOf(formData,result.Result.Id ) != -1)
                {

                    _ = LogoutAsync();
                }
                return Ok();

            }
            else if (formData[0] == "UnBlocked")
            {

                await UnBlockedUserAsync(formData);
                return Ok();
            }
            else if (formData[0] == "Delete")
            {
                var result = _userManager.FindByNameAsync(User.Identity.Name);
                if (Array.IndexOf(formData, result.Result.Id) != -1)
                {

                    _ = LogoutAsync();
                   

                }


                _ = await DeleteUser(formData);
                
                return Ok();

            }
            else if (formData[0] == "Logout")
            {

                 AccaunLogin = "";
                _ = LogoutAsync();
                


                return Ok();

            }

            return Ok();
        }


        public async Task<IActionResult> BlockedUserAsync(string[] userid)
        {
            foreach (var id in userid)
            {
                Task<User> user = _userManager.FindByIdAsync(id);
                if (user.Result == null)
                    continue;
                user.Result.LockoutEnabled = false;
                await _userManager.UpdateAsync(user.Result);
            }
            return Ok();
        }

        public async Task<IActionResult> UnBlockedUserAsync(string[] userid)
        {
            foreach (var id in userid)
            {
                Task<User> user = _userManager.FindByIdAsync(id);
                if (user.Result == null)
                    continue;
                user.Result.LockoutEnabled = true;
                await _userManager.UpdateAsync(user.Result);
            }
            return Ok();
        }

        public async Task<IActionResult>  DeleteUser(string[] userid)
        {
            foreach (var id in userid)
            {
                Task<User> user = _userManager.FindByIdAsync(id);
                if (user.Result == null)
                    continue;

                await  _userManager.DeleteAsync(user.Result);
            }
            return Ok();
        }
        

        public async Task<ActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");


        }
    }
}

       

