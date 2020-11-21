using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using LookMemories_WEB.Common;
using LookMemories_WEB.Interfaces;
using LookMemories_WEB.Model.DataBase;
using LookMemories_WEB.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NToastNotify;

namespace LookMemories_WEB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IToastNotification toastNotification;
        private readonly IPhoto photoRepository;
        private readonly IAlbums albumRepository;
        private readonly UserManager<AccountUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;

        /// <summary>
        /// ASSIGN needed param for Admin role
        /// </summary>
        /// <param name="ToastNotification"></param>
        /// <param name="PhotoRepository"></param>
        /// <param name="AlbumRepository"></param>
        /// <param name="UserManager"></param>
        /// <param name="HostingEnvironment"></param>
        /// <param name="Configuration"></param>
        public AdminController(IToastNotification ToastNotification, IPhoto PhotoRepository, IAlbums AlbumRepository, UserManager<AccountUser> UserManager, IWebHostEnvironment HostingEnvironment, IConfiguration Configuration)
        {
            toastNotification = ToastNotification;
            photoRepository = PhotoRepository;
            albumRepository = AlbumRepository;
            userManager = UserManager;
            hostingEnvironment = HostingEnvironment;
            configuration = Configuration;
        }

        /// <summary>
        /// ALL users
        /// </summary>
        /// <returns>view</returns>
        public async Task<IActionResult> AllUsers()
        {
            AccountUser user = await userManager.GetUserAsync(User);
            return View(userManager.Users.Where(u => u.Id != user.Id).ToList());
        }

        /// <summary>
        /// EDIT any user view
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>view</returns>
        public async Task<IActionResult> EditUser(string UserId)
        {
            EditProfile profile = new EditProfile();

            AccountUser user = await userManager.FindByIdAsync(UserId);
            profile.UserProfileVM = new EidtUserProfileViewModel();
            profile.UserProfileVM.Id = user.Id;
            profile.UserProfileVM.AboutMe = user.AboutMe;
            profile.UserProfileVM.FName = user.FirstName;
            profile.UserProfileVM.LName = user.LastName;
            profile.UserProfileVM.Gender = user.Gender;
            profile.UserProfileVM.DOB = user.DOB;
            profile.UserProfileVM.Email = user.Email;
            profile.UserProfileVM.UserName = user.UserName;

            profile.SQVM = new SecurityQuestionViewModel();
            profile.SQVM.UserId = user.Id;
            profile.SQVM.QuestionId = user.SecurityQuestion;
            profile.SQVM.Answer = user.Answer;
            profile.SQVM.SecurityQuestionsList = new SecurityQuestions().GetSecurityQuestion();

            profile.RSVM = new ResetPasswordViewModel();
            return View(profile);
        }

        /// <summary>
        /// DELETE current user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>all users view</returns>
        public async Task<IActionResult> DeleteUser(string UserId)
        {

            AccountUser user = await userManager.Users.Include(i => i.AlbumList).ThenInclude(r => r.PhotoList).Where(user => user.Id == UserId).FirstOrDefaultAsync();
            //user.AlbumList =   user.AlbumList.ToList();
            //   List<Album> albums=    albumRepository.GetUserAlbums(user.Id);
            if (user != null && user.AlbumList.Count > 0)
            {
                foreach (var item in user.AlbumList.ToList())
                {
                    foreach (var item2 in item.PhotoList.ToList())
                    {
                        LMCommon.DeletePhoto(item2.PhotoPath, hostingEnvironment.WebRootPath, configuration["PhotoPath"]);
                        photoRepository.DeletePhoto(item2.Id);
                    }
                    albumRepository.Delete(item.Id);
                }
            }

            await userManager.DeleteAsync(user);
            return RedirectToAction("AllUsers");
        }



        /// <summary>
        /// UPDATE secqurity questions of exixting user
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>updated view</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateSQ(EditProfile obj)
        {
            SecurityQuestionViewModel model = obj.SQVM;

            if (ModelState.IsValid)
            {
                AccountUser user = new AccountUser();

                user = await userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    user.SecurityQuestion = model.QuestionId;
                    user.Answer = model.Answer;


                    var res = await userManager.UpdateAsync(user);
                    if (res.Succeeded)
                    {
                        toastNotification.AddSuccessToastMessage("Security Question Update successfully");
                    }
                    else
                    {
                        toastNotification.AddErrorToastMessage("Some issue to update Security Question ");
                        TempData["Error"] = "Error";

                    }
                }
            }
            return RedirectToAction("EditUser", new RouteValueDictionary(
new { controller = "Admin", action = "EditUser", UserId = model.UserId }));

        }

        /// <summary>
        /// EDIT any profile of exixting user
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(EditProfile obj)
        {
            EidtUserProfileViewModel model = obj.UserProfileVM;

            if (ModelState.IsValid)
            {
                AccountUser user = new AccountUser();

                user = await userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.FirstName = model.FName;
                    user.LastName = model.LName;
                    user.Gender = model.Gender;
                    user.AboutMe = model.AboutMe;
                    user.DOB = model.DOB;

                    var res = await userManager.UpdateAsync(user);
                    if (res.Succeeded)
                    {
                        toastNotification.AddSuccessToastMessage("Profile Update successfully");
                    }
                    else
                    {
                        toastNotification.AddErrorToastMessage("Some issue to update Profile");
                        ModelState.AddModelError(String.Empty, "Error");
                    }
                }
            }

            return RedirectToAction("EditUser", new RouteValueDictionary(
new { controller = "Admin", action = "EditUser", UserId = model.Id }));

        }
    }
}
