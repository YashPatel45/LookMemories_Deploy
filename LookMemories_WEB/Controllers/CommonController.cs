using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LookMemories_WEB.Common;
using LookMemories_WEB.Interfaces;
using LookMemories_WEB.Model.ViewModel;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace LookMemories_WEB.Controllers
{
    public class CommonController : Controller
    {
        private readonly IAlbums albums;
        private readonly IPhoto photoRepository;
        public IDataProtector Protector;

        /// <summary>
        /// COMMON controllers for each album
        /// </summary>
        /// <param name="Albums"></param>
        /// <param name="dataProtectionProvider"></param>
        /// <param name="dataProtectionPurpose"></param>
        /// <param name="PhotoRepository"></param>
        public CommonController(IAlbums Albums, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeString dataProtectionPurpose, IPhoto PhotoRepository)
        {
            albums = Albums;
            DataProtectionProvider = dataProtectionProvider;
            DataProtectionPurpose = dataProtectionPurpose;
            photoRepository = PhotoRepository;
            Protector = DataProtectionProvider.CreateProtector(DataProtectionPurpose.PurposeString);
        }

        /// <summary>
        /// SHARE album with valid user
        /// </summary>
        public IDataProtectionProvider DataProtectionProvider { get; }
        public DataProtectionPurposeString DataProtectionPurpose { get; }

        public IActionResult Share(string albumId, string imgId)
        {

            ShareViewModel vm = new ShareViewModel();

            try
            {
                if (!String.IsNullOrEmpty(albumId))
                {
                    int AlbumId = Convert.ToInt32(Protector.Unprotect(albumId));
                    vm.Album = albums.GetById(AlbumId);
                }
                else if (!String.IsNullOrEmpty(imgId))
                {
                    int img = Convert.ToInt32(Protector.Unprotect(imgId));
                    vm.Photo = photoRepository.GetById(img);


                }
                else
                {
                    return BadRequest("URL is not correct");

                }

            }
            catch (Exception ex)
            {
                return BadRequest("URL is not correct");
            }
            return View(vm);
        }
    }
}
