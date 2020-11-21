using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LookMemories_WEB.Common;
using LookMemories_WEB.Interfaces;
using LookMemories_WEB.Model.DataBase;
using LookMemories_WEB.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using NToastNotify;

namespace LookMemories_WEB.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class UserController : Controller
    {
        private readonly IToastNotification toastNotification;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPhoto photoRepository;
        private readonly UserManager<AccountUser> userManager;
        private readonly IAlbums albumRepository;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;
        public IDataProtector Protector;
        public IDataProtectionProvider DataProtectionProvider { get; }
        public DataProtectionPurposeString DataProtectionPurpose { get; }

        /// <summary>
        /// ASSIGN needed parameters for user functionality
        /// </summary>
        /// <param name="ToastNotification">Gives good notification pop-up for success and falier msg</param>
        /// <param name="HttpContextAccessor"></param>
        /// <param name="PhotoRepository"></param>
        /// <param name="dataProtectionProvider"></param>
        /// <param name="dataProtectionPurpose"></param>
        /// <param name="UserManager"></param>
        /// <param name="albums"></param>
        /// <param name="HostingEnvironment"></param>
        /// <param name="Configuration"></param>
        public UserController(IToastNotification ToastNotification, IHttpContextAccessor HttpContextAccessor, IPhoto PhotoRepository, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeString dataProtectionPurpose, UserManager<AccountUser> UserManager, IAlbums albums, IWebHostEnvironment HostingEnvironment, IConfiguration Configuration)
        {
            toastNotification = ToastNotification;
            httpContextAccessor = HttpContextAccessor;
            photoRepository = PhotoRepository;
            DataProtectionProvider = dataProtectionProvider;
            DataProtectionPurpose = dataProtectionPurpose;
            Protector = DataProtectionProvider.CreateProtector(DataProtectionPurpose.PurposeString);
            userManager = UserManager;
            albumRepository = albums;
            hostingEnvironment = HostingEnvironment;
            configuration = Configuration;
        }

        /// <summary>
        /// CHECK user upload a correct file of photos
        /// </summary>
        /// <param name="files"></param>
        /// <param name="fileext"></param>
        /// <returns></returns>
        private bool ValidateFileExtension(List<IFormFile> files, out string fileext)
        {
            bool isvalid = true;
            string f = String.Empty;
            string str = configuration["FileExtensions"];
            str = str.ToLower();
            string[] fileex = str.Split(',');

            foreach (var item in files)
            {
                int startindex = item.FileName.LastIndexOf('.') + 1;
                int Length = item.FileName.Length;
                string ext = item.FileName.Substring(startindex, Length - startindex);
                if (!fileex.Contains(ext.ToLower()))
                {
                    isvalid = false;
                    f = ext;
                    break;
                }
            }
            fileext = f;
            return isvalid;
        }

        /// <summary>
        /// LOGIN with correcr user
        /// </summary>
        /// <returns>current user</returns>
        private async Task<AccountUser> GetLoggedInUser()
        {
            return await userManager.GetLoginAcccountUser(User);
        }

        /// <summary>
        /// Index view
        /// </summary>
        /// <returns>view</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET PROFILE info of user
        /// </summary>
        /// <returns>view</returns>
        public async Task<IActionResult> Profile()
        {
            AccountUser user = new AccountUser();
            if (User.Identity.IsAuthenticated)
            {
                user = await userManager.GetUserAsync(User);
                if (user != null && user.SecurityQuestion > 0)
                {
                    user.SecurityQuestionName = new SecurityQuestions().GetSecurityQuestion().Where(s => s.Id == user.SecurityQuestion).FirstOrDefault().Question.ToString();
                }
            }
            return View(user);
        }

        /// <summary>
        /// EDIT profile of current USER
        /// </summary>
        /// <returns>view</returns>
        public async Task<IActionResult> EditProfile()
        {
            //ERROR if something went wrong
            if (TempData["Errors"] != null)
            {
                ViewBag.Errors = TempData["Errors"].ToString();
            }

            EditProfile profile = new EditProfile();
            AccountUser user = new AccountUser();

            //EDIT any filed 
            if (User.Identity.IsAuthenticated)
            {
                user = await userManager.GetUserAsync(User);
                profile.UserProfileVM = new EidtUserProfileViewModel();
                profile.UserProfileVM.AboutMe = user.AboutMe;
                profile.UserProfileVM.FName = user.FirstName;
                profile.UserProfileVM.LName = user.LastName;
                profile.UserProfileVM.Gender = user.Gender;
                profile.UserProfileVM.DOB = user.DOB;
                profile.UserProfileVM.Email = user.Email;
                profile.UserProfileVM.UserName = user.UserName;

                profile.SQVM = new SecurityQuestionViewModel();
                profile.SQVM.QuestionId = user.SecurityQuestion;
                profile.SQVM.Answer = user.Answer;
                profile.SQVM.SecurityQuestionsList = new SecurityQuestions().GetSecurityQuestion();

                profile.RSVM = new ResetPasswordViewModel();
            }
            return View(profile);
        }

        /// <summary>
        /// UPDATE profile information(First,Last Name, Gender, About me, DOB) after Editing completed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>edit profile view</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(EditProfile obj)
        {
            EidtUserProfileViewModel model = obj.UserProfileVM;

            if (ModelState.IsValid)
            {
                AccountUser user = new AccountUser();
                //IF user identity is correct proceed to update
                if (User.Identity.IsAuthenticated)
                {
                    user = await userManager.GetUserAsync(User);
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
                            toastNotification.AddSuccessToastMessage("Profile Update Successfully.");
                        }
                        //GIVE an error if something is wrong
                        else
                        {
                            toastNotification.AddErrorToastMessage("Some issue to updating profile.");
                            ModelState.AddModelError(String.Empty, "Error");
                        }
                    }
                }
            }
            return RedirectToAction("EditProfile");
        }

        /// <summary>
        /// UPDATE Security question information
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>edit profile view</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateSQ(EditProfile obj)
        {
            SecurityQuestionViewModel model = obj.SQVM;

            if (ModelState.IsValid)
            {
                AccountUser user = new AccountUser();
                //IF user is correct and inputs are correct proceed to update 
                if (User.Identity.IsAuthenticated)
                {
                    user = await userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        user.SecurityQuestion = model.QuestionId;
                        user.Answer = model.Answer;

                        var res = await userManager.UpdateAsync(user);
                        if (res.Succeeded)
                        {
                            toastNotification.AddSuccessToastMessage("Security Question Update Successfully.");
                        }
                        //GIVE an error if something is wrong
                        else
                        {
                            toastNotification.AddSuccessToastMessage("Some issue to updating Security Question.");
                            ModelState.AddModelError(String.Empty, "Error");
                        }
                    }
                }

            }

            return RedirectToAction("EditProfile");
        }

        /// <summary>
        /// SHARE album with particular user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>successful message</returns>
        [HttpPost]
        public async Task<JsonResult> ShareImages(ShareAlbum model)
        {
            string host = httpContextAccessor.HttpContext.Request.Host.Value;
            string Path = host + "/Common/Share";

            if (model.IsAlbum)
            {
                Path = Path + "?albumId=" + model.SId;
            }
            else
            {
                Path = Path + "?imgId=" + model.SId;
            }
            Path = "<a href='" + Path + "' >" + Path + "</a>";

            //GET credentials of particular user to share album
            bool issend = LookMemoriesSharing.SendEmail("Look Memories", Path, model.SEmailId, configuration["SenderEmialId"], configuration["Password"]);

            //SHARE album if successful
            if (issend)
            {
                toastNotification.AddSuccessToastMessage("Album share Succesfully");
            }
            //GIVE an Error if unsuccessful
            else
            {
                toastNotification.AddErrorToastMessage("There is some issue, kindly write the correct email address.");
            }
            return Json(issend);
        }

        /// <summary>
        /// RESET password
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Edit profile view</returns>
        [HttpPost]
        public async Task<IActionResult> ResetPassword(EditProfile obj)
        {
            ResetPasswordViewModel model = obj.RSVM;

            //CHECK user identity  and proceed to reset password
            if (ModelState.IsValid)
            {
                AccountUser user = new AccountUser();
                if (User.Identity.IsAuthenticated)
                {
                    user = await userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        if (await userManager.CheckPasswordAsync(user, model.OldPassword))
                        {
                            string token = await userManager.GeneratePasswordResetTokenAsync(user);

                            var result = await userManager.ResetPasswordAsync(user, token, model.Password);

                            if (result.Succeeded)
                            {
                                toastNotification.AddSuccessToastMessage("Password Reset Successfully.");
                                return RedirectToAction("EditProfile");
                            }
                            //GIVE an error if something is wrong
                            else
                            {
                                toastNotification.AddErrorToastMessage("Some issue to reset password.");
                            }

                            string errors = String.Empty;
                            foreach (var item in result.Errors)
                            {
                                errors = errors + item.Description + "</br>";
                            }
                            TempData["Errors"] = errors;
                        }
                    }
                }
            }
            return RedirectToAction("EditProfile");
        }

        /// <summary>
        /// MY ALBUM view - shows each album
        /// </summary>
        /// <returns>view</returns>
        public async Task<IActionResult> MyAlbums()
        {
            List<Album> model = new List<Album>();

            AccountUser user = await GetLoggedInUser();
            model = albumRepository.GetUserAlbums(user.Id);
            model.Select(p => { p.ProtectorId = Protector.Protect(p.Id.ToString()); return p; }).ToList();

            return View(model);
        }

        /// <summary>
        /// NEW ALBUM view  
        /// </summary>
        /// <returns>view</returns>
        public async Task<IActionResult> NewAlbum()
        {
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"].ToString();
            }

            NewAlbumViewModel model = new NewAlbumViewModel();
            AccountUser user = await GetLoggedInUser();
            model.AlbumList = albumRepository.GetUserAlbums(user.Id);

            model.AlbumList.Select(p => { p.ProtectorId = Protector.Protect(p.Id.ToString()); return p; }).ToList();
            model.Album = new Album();

            return View(model);
        }

        /// <summary>
        /// GET Image path
        /// </summary>
        /// <param name="imgPath"></param>
        /// <returns>Image name</returns>
        private string GetImageName(string imgPath)
        {
            string imgName = String.Empty;
            int fst = imgPath.LastIndexOf('_');
            int lst = imgPath.LastIndexOf('.');
            imgName = imgPath.Substring((fst + 1), (lst - fst) - 1);
            return imgName;
        }

        /// <summary>
        /// CREATE new album - where user can create album
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>New Album view</returns>
        [HttpPost]
        public async Task<IActionResult> NewAlbum(NewAlbumViewModel obj)
        {
            Album model = obj.Album;

            //IF model state is valid then procced
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    model.CreatedDate = DateTime.Now.ToString();
                }

                AccountUser user = await GetLoggedInUser();
                model.AccountUserId = user.Id;

                if (model.FileLists?.Count > 0)
                {
                    string FileExtension = String.Empty;

                    //CREATE album with valid images or gives an ERROR
                    if (!ValidateFileExtension(model.FileLists, out FileExtension))
                    {
                        TempData["Error"] = "File Extension " + FileExtension + " is not allowed";
                        toastNotification.AddErrorToastMessage("File Extension " + FileExtension + " is not allowed");
                        return RedirectToAction("NewAlbum");
                    }

                    List<Photos> p = new List<Photos>();
                    List<string> Paths = new List<string>();
                    Paths = await LMCommon.UploadMultiplePhoto(model.FileLists, hostingEnvironment.WebRootPath, configuration["PhotoPath"]);
                    if (Paths.Count > 0)
                    {
                        foreach (var item in Paths)
                        {
                            Photos ph = new Photos();
                            ph.PhotoPath = item;
                            ph.UserId = user.Id;
                            ph.ImgName = GetImageName(item);
                            ph.UplodedDate = DateTime.Now.ToString();
                            p.Add(ph);
                        }
                    }
                    model.PhotoList = p;
                }
                albumRepository.Add(model);
                toastNotification.AddSuccessToastMessage("Album save successfully");
            }
            return RedirectToAction("NewAlbum");
        }

        /// <summary>
        /// UPDATE album 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>New Album view</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateAlbum(Album model)
        {
            //IF album is valid then proceed
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    model.CreatedDate = DateTime.Now.ToString();
                }
                AccountUser user = await GetLoggedInUser();
                model.AccountUserId = user.Id;
                if (model.FileLists?.Count > 0)
                {
                    string FileExtension = String.Empty;

                    //CHECK if file uploaded is wrong
                    if (!ValidateFileExtension(model.FileLists, out FileExtension))
                    {
                        //IF wrong file give an ERROR message
                        TempData["Error"] = "File Extension " + FileExtension + " is not allowed";
                        toastNotification.AddErrorToastMessage("File Extension " + FileExtension + " is not allowed");
                        return RedirectToAction("AlbumDetail", new RouteValueDictionary(new { controller = "User", action = "AlbumDetail", albumId = model.ProtectorId }));
                    }
                    List<Photos> p = new List<Photos>();
                    List<string> Paths = new List<string>();
                    Paths = await LMCommon.UploadMultiplePhoto(model.FileLists, hostingEnvironment.WebRootPath, configuration["PhotoPath"]);
                    if (Paths.Count > 0)
                    {
                        foreach (var item in Paths)
                        {
                            Photos ph = new Photos();
                            ph.PhotoPath = item;
                            ph.UserId = user.Id;
                            ph.UplodedDate = DateTime.Now.ToString();
                            ph.ImgName = GetImageName(item);

                            p.Add(ph);
                        }
                    }
                    model.PhotoList = p;
                }
                //SUCCESSFULY updated
                albumRepository.Add(model);
                toastNotification.AddSuccessToastMessage("Album update successfully");
            }

            return RedirectToAction("NewAlbum");
        }

        /// <summary>
        /// GET album details
        /// </summary>
        /// <param name="albumId"></param>
        /// <returns>view</returns>
        public async Task<IActionResult> AlbumDetail(string albumId)
        {
            //GIVES an ERROR if null
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"].ToString();

            }
            AccountUser user = await GetLoggedInUser();
            int AlbumId = 0;
            Album model = new Album();
            try
            {
                if (!String.IsNullOrEmpty(albumId))
                {
                    AlbumId = Convert.ToInt32(Protector.Unprotect(albumId));
                    model = albumRepository.GetUserAlbumById(user.Id, AlbumId);
                    ViewBag.AlbumEncryptedId = albumId;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            model.ProtectorId = albumId;
            return View(model);
        }

        /// <summary>
        /// CREATE FAVORITE section
        /// </summary>
        /// <returns>Photo list view</returns>
        public async Task<IActionResult> Favorites()
        {
            List<Photos> PhotoList = new List<Photos>();
            AccountUser user = await GetLoggedInUser();
            PhotoList = photoRepository.GetAllFavPhotosByUserId(user.Id);
            return View(PhotoList);
        }

        /// <summary>
        /// UPDATE comments on image
        /// </summary>
        /// <param name="photos"></param>
        /// <returns>Updated album details view</returns>
        [HttpPost]
        public JsonResult UpdateComment(Photos photos)
        {
            Photos p = photoRepository.ChangeComment(photos);
            return Json(p);
        }

        /// <summary>
        /// SAVE comment and Image name
        /// </summary>
        /// <param name="photos"></param>
        /// <returns>Updated album details view</returns>
        [HttpPost]
        public JsonResult UpdateCommentandImgName(Photos photos)
        {
            Photos p = photoRepository.UpdateCommentAndImgName(photos);
            toastNotification.AddSuccessToastMessage("Data Saved Successfully");
            return Json(p);

        }

        /// <summary>
        /// ADD images to FAVORITE section
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="isFav"></param>
        /// <param name="EncrytedId"></param>
        /// <returns>Album Details view</returns>
        [HttpGet]
        public IActionResult AddToFav(int Id, bool isFav, string EncrytedId)
        {
            Photos pho = new Photos() { Id = Id, IsFav = isFav };
            Photos p = photoRepository.ChangeFavStatus(pho);
            toastNotification.AddSuccessToastMessage("Image added into favourites");
            return RedirectToAction("AlbumDetail", new RouteValueDictionary(new { controller = "User", action = "AlbumDetail", albumId = EncrytedId }));
        }

        /// <summary>
        /// DELETE image
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="AlbumEncrypted"></param>
        /// <returns>Album Details view</returns>
        [HttpGet]
        public IActionResult DeletePhoto(int Id, string AlbumEncrypted)
        {
            Photos p = photoRepository.DeletePhoto(Id);
            //return RedirectToAction("AlbumDetail", new RouteValueDictionary(new { controller = "User", action = "AlbumDetail", albumId = AlbumEncrypted }));

            LMCommon.DeletePhoto(p.PhotoPath, hostingEnvironment.WebRootPath, configuration["PhotoPath"]);

            toastNotification.AddSuccessToastMessage("Image Delete Successfully");
            return RedirectToAction("AlbumDetail", new RouteValueDictionary(new { controller = "User", action = "AlbumDetail", albumId = AlbumEncrypted }));

        }


        /// <summary>
        /// ALL photos view - show all photo
        /// </summary>
        /// <returns>All Photos view</returns>
        public async Task<IActionResult> AllPhotos()
        {
            AccountUser user = await GetLoggedInUser();

            List<Photos> AllPhotos = photoRepository.GetAllPhotosByUserId(user.Id);
            return View(AllPhotos);
        }

        /// <summary>
        /// ALL favorite picture view - show all favorite images
        /// </summary>
        /// <returns>Photo list</returns>
        public async Task<IActionResult> UserFavorites()
        {
            List<Photos> PhotoList = new List<Photos>();
            AccountUser user = await GetLoggedInUser();

            PhotoList = photoRepository.GetAllFavPhotosByUserId(user.Id);

            return View(PhotoList);
        }

        /// <summary>
        /// ALL albums view - show all albums
        /// </summary>
        /// <returns>All Albums view</returns>
        public async Task<IActionResult> AllAlbums()
        {
            List<Album> AlbumList = new List<Album>();
            AccountUser user = await GetLoggedInUser();
            AlbumList = albumRepository.GetUserAlbums(user.Id);
            AlbumList.Select(p => { p.ProtectorId = Protector.Protect(p.Id.ToString()); return p; }).ToList();

            return View(AlbumList);
        }

        /// <summary>
        /// SEARCH image
        /// </summary>
        /// <param name="q"></param>
        /// <returns>photo list</returns>
        public async Task<IActionResult> SearchMemories(string q)
        {
            if (q == null) return View(new List<Photos>());
            ViewBag.q = q;

            List<Photos> PhotosList = new List<Photos>();
            AccountUser user = await GetLoggedInUser();

            PhotosList = photoRepository.GetPhotosBySearch(q, user.Id);
            return View(PhotosList);

        }

        /// <summary>
        /// DOWNLOAD album
        /// </summary>
        /// <param name="albumId"></param>
        /// <returns></returns>
        public JsonResult DownloadAlbum(int albumId)
        {
            try
            {
                DownloadAlbum al = albumRepository.DownloadAlbum(albumId);
                foreach (var item in al.PhotoList)
                {
                    item.PhotoPath = "../" + configuration["PhotoPath"].ToString() + "/" + item.PhotoPath;
                }

                return Json(al);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// REPLACE exixting picture
        /// </summary>
        /// <param name="model"></param>
        /// <returns>album details</returns>
        [HttpPost]
        public async Task<IActionResult> ReplacePhoto(Photos model)
        {
            if (model != null)
            {
                string FileExtension = String.Empty;

                List<IFormFile> filelst = new List<IFormFile>();
                filelst.Add(model.ReplacePhoto);

                //CHECK for new file if it's valid then proceed
                if (!ValidateFileExtension(filelst, out FileExtension))
                {
                    TempData["Error"] = "File Extension " + FileExtension + " is not allowed";
                    return RedirectToAction("AlbumDetail", new RouteValueDictionary(new { controller = "User", action = "AlbumDetail", albumId = model.AlbumEncrypted }));
                }

                Photos p = photoRepository.DeletePhoto(model.Id);
                //return RedirectToAction("AlbumDetail", new RouteValueDictionary(new { controller = "User", action = "AlbumDetail", albumId = AlbumEncrypted }));

                LMCommon.DeletePhoto(model.PhotoPath, hostingEnvironment.WebRootPath, configuration["PhotoPath"]);

                string path = await LMCommon.UploadPhoto(model.ReplacePhoto, hostingEnvironment.WebRootPath, configuration["PhotoPath"]);

                //REPLACE 
                if (!String.IsNullOrEmpty(path))
                {

                    AccountUser user = await GetLoggedInUser();
                    Photos ph = new Photos();
                    ph.PhotoPath = path;

                    ph.AlbumId = model.AlbumId;
                    ph.UserId = user.Id;
                    ph.UplodedDate = DateTime.Now.ToString();
                    ph.ImgName = GetImageName(path);

                    photoRepository.Add(ph);

                    toastNotification.AddSuccessToastMessage("Image Replace successfully");
                }
            }

            return RedirectToAction("AlbumDetail", new RouteValueDictionary(new { controller = "User", action = "AlbumDetail", albumId = model.AlbumEncrypted }));
        }

        /// <summary>
        /// DELETE whole album
        /// </summary>
        /// <param name="albumid"></param>
        /// <param name="pagename"></param>
        /// <returns>My Album view</returns>
        public IActionResult DeleteAlbum(string albumid, string pagename)
        {
            Album album = new Album();

            int AlbumId = 0;
            if (!String.IsNullOrEmpty(albumid))
            {
                AlbumId = Convert.ToInt32(Protector.Unprotect(albumid));
                album = albumRepository.GetById(AlbumId);
            }

            albumRepository.Delete(AlbumId);

            foreach (var item in album.PhotoList)
            {
                LMCommon.DeletePhoto(item.PhotoPath, hostingEnvironment.WebRootPath, configuration["PhotoPath"]);
            }

            toastNotification.AddSuccessToastMessage("Album Delete Successfully.");
            string page = pagename.ToLower();

            if (page == "allalbums")
            {
                return RedirectToAction("AllAlbums", "User");
            }
            else if (page == "newalbums")
            {
                return RedirectToAction("NewAlbum", "User");
            }

            return RedirectToAction("MyAlbums", "User");
            //          return RedirectToAction("AlbumDetail", new RouteValueDictionary(
            //new { controller = "User", action = "AlbumDetail", albumId = model.ProtectorId }));
        }

    }
}
