using LookMemories_WEB.Model.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Common
{
    public static class ExtensioMethods
    {
        static UserManager<AccountUser> manager;

        public static void Seed(this ModelBuilder builder)
        {
            //modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole("Candidate"), new IdentityRole("Employer"), new IdentityRole("Admin"));

            const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            const string USER_ROLE_ID = "5eba9226-5f58-4a2d-8c04-3126aefd3698";

            const string ADMIN_ROLE_ID = "690f679c-4431-41ad-b098-82fc4f8c4dea";

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ADMIN_ROLE_ID,
                Name = "Admin",
                NormalizedName = "Admin"
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = USER_ROLE_ID,
                Name = "User",
                NormalizedName = "User"
            });

            var hasher = new PasswordHasher<IdentityUser>();

            //ASSIGN admin role credentials
            builder.Entity<AccountUser>().HasData(new AccountUser
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, "Admin@123"),
                SecurityStamp = string.Empty,
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ADMIN_ROLE_ID,
                //  RoleId = 3.ToString(),
                UserId = ADMIN_ID
            });



            //   builder.Entity<Photos>()
            //.HasOne(p => p.Album)
            //.WithMany(b => b.PhotoList)
            //    .HasForeignKey(p => p.AlbumId);



            //   builder.Entity<Job>()
            //          .HasOne(p => p.AccountUser)
            //          .WithMany(b => b.EmployerUserList)
            //              .HasForeignKey(p => p.AccountUserId);
        }

        public static T Clone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}
