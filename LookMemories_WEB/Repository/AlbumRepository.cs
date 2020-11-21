using LookMemories_WEB.Interfaces;
using LookMemories_WEB.Model.DataBase;
using LookMemories_WEB.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Repository
{
    public class AlbumRepository : IAlbums
    {
        //ASSIGN dbContext to create albums
        private readonly DBContext context;

        //CREATE album repository
        public AlbumRepository(DBContext context)
        {
            this.context = context;
        }

        //CREATE album 
        public bool Add(Album album)
        {
            try
            {
                if (album.Id > 0)
                {
                    Album a = context.Albums.FirstOrDefault(context => context.Id == album.Id);
                    a.AlbumName = album.AlbumName;
                    a.PhotoList = album.PhotoList;
                    context.Entry(a).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    context.Albums.Add(album);
                    context.SaveChanges();
                    return true;

                }
            }
            catch (Exception ex)
            {

                return false;
            }
    
        }

        //GET album by it's ID
        public Album GetUserAlbumById(string UserId, int Id)
        {
            try
            {
                using (context)
                {
                  return  context.Albums.Include(i => i.PhotoList).AsNoTracking().Where(c => c.AccountUserId == UserId && c.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        //GET ID 
        public Album GetById(int Id)
        {
            try
            {
                using (context)
                {
                    return context.Albums.Include(i => i.PhotoList).AsNoTracking().Where(c =>  c.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        //DOWNLOAD album
        public DownloadAlbum DownloadAlbum(int Id)
        {
            try
            {
                using (context)
                {
                    return context.Albums.Include(i => i.PhotoList).Where(c => c.Id == Id).AsNoTracking()
                        .Select(a => new DownloadAlbum
                        {
                            AlbumName = a.AlbumName,
                            
                            PhotoList =a.PhotoList


                        }).FirstOrDefault();   
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        //GET ALL albums
        public List<Album> GetUserAlbums(string UserId)
        {
            using (context)
            {
                return context.Albums.Include(i => i.PhotoList).AsNoTracking().Where(c => c.AccountUserId == UserId).ToList();
            }
        }

        //DELETE whole album
        public void Delete(int Id)
        {
            using (context)
            {
                Album p = context.Albums.FirstOrDefault(p => p.Id == Id);
                context.Remove(p);
                context.SaveChanges();

            }
        }
    }
}
