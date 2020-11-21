using LookMemories_WEB.Interfaces;
using LookMemories_WEB.Model.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Repository
{
    public class PhotoRepository : IPhoto
    {
        //ASSIGN dbContext to create upload photos
        private readonly DBContext context;

        //CREATE photo repositorys into db
        public PhotoRepository(DBContext dBContext)
        {
            this.context = dBContext;
        }

        //ADD/CHANGE comment on photo
        public Photos ChangeComment(Photos model)
        {
            using (context)
            {
             Photos p =   context.Photos.FirstOrDefault(p => p.Id == model.Id);
                p.Comment = model.Comment;

                context.Entry(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
                return p;
            }
        }

        //UPDATE comment and IMAGE name
        public Photos UpdateCommentAndImgName(Photos model)
        {
            using (context)
            {
                Photos p = context.Photos.FirstOrDefault(p => p.Id == model.Id);
                p.Comment = model.Comment;
                p.ImgName = model.ImgName;
                context.Entry(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();

                return p;
            }
        }

        //SAVE changed comment or image ID 
        public Photos ChangeFavStatus(Photos model)
        {
            using (context)
            {
                Photos p = context.Photos.FirstOrDefault(p => p.Id == model.Id);
                p.IsFav = model.IsFav;

                context.Entry(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();

                return p;
            }
        }

        //DELETE photo
        public Photos DeletePhoto(int Id)
        {
            using (context)
            {
                Photos p = context.Photos.FirstOrDefault(p => p.Id == Id);
                context.Remove(p);
                context.SaveChanges();

                return p;
            }
        }

        //GET all photos which are added in fav
        public List<Photos> GetAllFavPhotosByUserId(string UserId)
        {
            using (context)
            {
               return context.Photos.Where(p => p.IsFav == true && p.UserId == UserId).ToList();

            }
        }

        //GET all photos with it's ID name
        public List<Photos> GetAllPhotosByUserId(string UserId)
        {
            using (context)
            {
                return context.Photos.Where(p => p.UserId == UserId).ToList();

            }
        }

        //SEARCH photo by it's ID 
        public List<Photos> GetPhotosBySearch(string query, string UserId)
        {
            using (context)
            {
                return context.Photos.Where(p => p.UserId == UserId && p.ImgName.ToLower().Contains(query.ToLower())).ToList();

            }
        }

        //GET photo ID to search
        public Photos GetById(int id) {

            using (context)
            {
                return context.Photos.FirstOrDefault(c => c.Id == id);
            }
        
        
        }

        //ADD photos 
        public Photos Add(Photos p)
        {
            try
            {
                using (context)
                {
                    context.Photos.Add(p);
                    context.SaveChanges();

                    return p;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}
