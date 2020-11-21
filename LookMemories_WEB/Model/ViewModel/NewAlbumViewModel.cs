using LookMemories_WEB.Model.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Model.ViewModel
{
    //VIEWMODEL for New Album
    public class NewAlbumViewModel
    {
        public List<Album> AlbumList { get; set; }

        public Album Album { get; set; }
    }
}
