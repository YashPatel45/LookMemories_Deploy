using LookMemories_WEB.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Model.DataBase
{
    public class DBContext : IdentityDbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
     
        }

        //CALL OnModelCreating method to build the model 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        
            builder.Seed();
        }

       
        //CREATE db for albums
        public DbSet<Album> Albums { get; set; }

        //CREATE db for photos
        public DbSet<Photos> Photos { get; set; }
    }
}
