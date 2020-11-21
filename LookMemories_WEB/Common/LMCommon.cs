
using LookMemories_WEB.Model.DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LookMemories_WEB.Common
{
    public static class LMCommon
    {

        public async static Task<AccountUser> GetLoginAcccountUser(this UserManager<AccountUser> userManager, ClaimsPrincipal user)
        {
            AccountUser u = null;
            u = await userManager.GetUserAsync(user);

            return u;
        }

        public static async Task<string> UploadPhoto(IFormFile file, string Rootpath, string FolderName)
        {
            string FilePath = String.Empty;
            var UniqueFileName = String.Empty;

            if (file != null)
            {
                var FileName = file.FileName;
                UniqueFileName = Guid.NewGuid() + "_" + file.FileName;
                FilePath = Path.Combine(Rootpath, FolderName, UniqueFileName);
                using (FileStream stream = new FileStream(FilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            return UniqueFileName;
        }

        public static async Task<List<string>> UploadMultiplePhoto(List<IFormFile> files, string Rootpath, string FolderName)
        {
            List<string> PhotoPathList = new List<string>();
            string FilePath = String.Empty;
            var UniqueFileName = String.Empty;

            if (files.Count > 0)
            {
                foreach (IFormFile item in files)
                {
                    var FileName = item.FileName;
                    UniqueFileName = Guid.NewGuid() + "_" + item.FileName;
                    FilePath = Path.Combine(Rootpath, FolderName, UniqueFileName);
                    using (FileStream stream = new FileStream(FilePath, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                    PhotoPathList.Add(UniqueFileName);
                }
            }
            return PhotoPathList;
        }

        public static bool DeletePhoto(string fileName, string Rootpath, string FolderName)
        {
            var path = Path.Combine(Rootpath, FolderName, fileName);
            File.Delete(path);

            return true;
        }
    }

    public class DataProtectionPurposeString
    {
        public readonly string PurposeString = "EncryptionRouteValueForProtectingRouteData";
    }
}
