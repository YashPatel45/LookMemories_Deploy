using LookMemories_WEB.Model.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Interfaces
{
    //CREATE INTERFACE for Photos
    public interface IPhoto
    {
        //CHANGE comments on each photo
        Photos ChangeComment(Photos model);

        //UPDATE comment and Image
        Photos UpdateCommentAndImgName(Photos model);

        //ADD photo to fav 
        Photos ChangeFavStatus(Photos model);

        //DELETE photo
        Photos DeletePhoto(int Id);

        //GET all photos
        List<Photos> GetAllPhotosByUserId(string UserId);

        //GET all fav photos
        List<Photos> GetAllFavPhotosByUserId(string UserId);

        //GET searched photos by ID  
        List<Photos> GetPhotosBySearch(string query, string UserId);

        //GET ID of photo
        Photos GetById(int id);

        //ADD photos
        Photos Add(Photos p);
    }
}
