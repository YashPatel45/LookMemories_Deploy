using LookMemories_WEB.Common;
using LookMemories_WEB.Interfaces;
using LookMemories_WEB.Model.DataBase;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Views.Shared.Components
{
    public class FavViewComponent : ViewComponent
    {
        private readonly IPhoto photoRepository;
        IDataProtector Protector;
        public FavViewComponent(IPhoto PhotoRepository , Microsoft.AspNetCore.Identity.UserManager<AccountUser> userManager,IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeString dataProtectionPurpose)
        {
            photoRepository = PhotoRepository;
            UserManager = userManager;
            
            DataProtectionProvider = dataProtectionProvider;
            DataProtectionPurpose = dataProtectionPurpose;
            Protector = DataProtectionProvider.CreateProtector(DataProtectionPurpose.PurposeString);
        }

       


        public UserManager<AccountUser> UserManager { get; }
        public IDataProtectionProvider DataProtectionProvider { get; }
        public DataProtectionPurposeString DataProtectionPurpose { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Photos> PhotoList = new List<Photos>();

            AccountUser user = await UserManager.GetLoginAcccountUser(UserClaimsPrincipal);
           

            PhotoList = photoRepository.GetAllFavPhotosByUserId(user.Id);


       


            return View("~/Views/Shared/_Fav.cshtml", PhotoList);
        }



       
    }
}
