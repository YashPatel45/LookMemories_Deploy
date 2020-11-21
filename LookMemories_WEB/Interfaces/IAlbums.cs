using LookMemories_WEB.Model.DataBase;
using LookMemories_WEB.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Interfaces
{
    //CREATE INTERFACE for albums
    public interface IAlbums
    {
        //ADD albums
        bool Add(Album album);

        //GET user albums
        List<Album> GetUserAlbums(string UserId);

        //GET album by ID
        Album GetUserAlbumById(string UserId, int Id);

        //DOWNLOAD album
        DownloadAlbum DownloadAlbum(int Id);

        //DELETE album
        void Delete(int Id);

        //GET album by ID
        Album GetById(int Id);
    }
}
