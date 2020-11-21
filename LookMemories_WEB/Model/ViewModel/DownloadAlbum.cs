using LookMemories_WEB.Model.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Model.ViewModel
{
    //VIEWMODEL for Download album
    public class DownloadAlbum
    {
        public string AlbumName { get; set; }

        public List<Photos> PhotoList { get; set; }
    }
}
