using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LookMemories_WEB.Common;
using LookMemories_WEB.Model;
using LookMemories_WEB.Model.DataBase;
using LookMemories_WEB.Model.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LookMemories_WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AccountUser> userManager;
        private readonly SignInManager<AccountUser> signInManager;

        /// <summary>
        /// ASSIGN needed param for user role
        /// </summary>
        /// <param name="UserManager"></param>
        /// <param name="SignInManager"></param>
        public AccountController(UserManager<AccountUser> UserManager, SignInManager<AccountUser> SignInManager)
        {
            userManager = UserManager;
            signInManager = SignInManager;
        }

        /// <summary>
        /// LOGIN view
        /// </summary>
        /// <returns>view</returns>
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// LOGIN the user - successful then show user Profile view
        /// ELSE show an ERROR
        /// </summary>
        /// <param name="model"></param>
        /// <returns>view</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                AccountUser accountUser = await userManager.FindByEmailAsync(model.Email);
                if (accountUser != null)
                {
                    var result = await signInManager.PasswordSignInAsync(accountUser, model.Password, model.IsRemember, false);
                    if (result.Succeeded)
                    {
                        if (User.IsInRole(RolesEnum.Admin.ToString()))
                        {
                            return RedirectToAction("AllUsers", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Profile", "User");
                        }

                    }

                }
                ModelState.AddModelError(string.Empty, "Email or Password is Incorrect");

            }
            return View(model);
        }

        /// <summary>
        /// REGISTRATION view
        /// </summary>
        /// <returns>view</returns>
        [HttpGet]
        public IActionResult Registration()
        {
            RegistrationViewModel vmodel = new RegistrationViewModel();
            vmodel = new RegistrationViewModel();
            vmodel.SecurityQuestionsList = new SecurityQuestions().GetSecurityQuestion();
            return View(vmodel);
        }

        /// <summary>
        /// REGISTER as a new User
        /// </summary>
        /// <param name="model"></param>
        /// <returns>view</returns>
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                AccountUser user = new AccountUser()
                {
                    UserName = model.UserName,
                    FirstName = model.FName,
                    LastName = model.LName,
                    Email = model.Email,
                    SecurityQuestion = model.QuestionId,
                    Answer = model.Answer
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    AccountUser accountUser = await userManager.FindByEmailAsync(model.Email);
                    await userManager.AddToRoleAsync(accountUser, RolesEnum.User.ToString());

                    await signInManager.PasswordSignInAsync(accountUser, model.Password, true, false);
                    return RedirectToAction("Profile", "User");

                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            };
            model.SecurityQuestionsList = new SecurityQuestions().GetSecurityQuestion();
            return View(model);
        }

        /// <summary>
        /// ASSIGN security question
        /// </summary>
        /// <returns>view</returns>
        public IActionResult Verification()
        {
            SecurityQuestionViewModel model = new SecurityQuestionViewModel();
            model.SecurityQuestionsList = new SecurityQuestions().GetSecurityQuestion();

            return View(model);
        }

        /// <summary>
        /// CHECK for user id and password - if valid proceed to reset password
        /// </summary>
        /// <param name="model"></param>
        /// <returns>view</returns>
        [HttpPost]
        public async Task<IActionResult> Verification(SecurityQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                AccountUser accountUser = await userManager.FindByEmailAsync(model.Email);
                if (accountUser != null)
                {
                    if (accountUser.SecurityQuestion == model.QuestionId && accountUser.Answer.ToLower() == model.Answer.ToLower())
                    {
                        TempData["UserId"] = accountUser.Id;
                        TempData["Email"] = accountUser.Email;

                        string token = await userManager.GeneratePasswordResetTokenAsync(accountUser);

                        TempData["Token"] = token;

                        return RedirectToAction("ResetPassword", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Email Address or Security Question is wrong");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email Address or Security Question is wrong");
                }
            }
            model.SecurityQuestionsList = new SecurityQuestions().GetSecurityQuestion();

            return View(model);
        }

        /// <summary>
        /// RESET password
        /// </summary>
        /// <returns>view</returns>
        public IActionResult ResetPassword()
        {

            if (TempData["UserId"] == null)
            {
                return RedirectToAction("Login");
            }

            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.UserId = TempData["UserId"].ToString();
            model.Email = TempData["Email"].ToString();
            model.Token = TempData["Token"].ToString();

            return View(model);
        }

        /// <summary>
        /// RESET password view shows error - if Confirm password is wrong
        /// </summary>
        /// <param name="model"></param>
        /// <returns>view</returns>
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                AccountUser accountUser = await userManager.FindByIdAsync(model.UserId);
                if (accountUser != null)
                {
                    var result = await userManager.ResetPasswordAsync(accountUser, model.Token, model.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                    foreach (var item in result.Errors)
                    {

                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User Not Found");
                }
            }
            return View(model);
        }

        /// <summary>
        /// LOGOUT the current user
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}
