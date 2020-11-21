using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LookMemories_WEB.Model.DataBase
{
    public class Photos
    {
        public int Id { get; set; }

        public string PhotoPath { get; set; }

        public string UplodedDate { get; set; }

        public string Comment { get; set; }
        
        public virtual Album Album { get; set; }
        public int AlbumId { get; set; }

        public bool IsFav { get; set; }

        public string UserId { get; set; }

        public string ImgName { get; set; }

        [NotMapped]
        public string AlbumEncrypted { get; set; }

        [NotMapped]
        public IFormFile ReplacePhoto { get; set; }

        [NotMapped]
        public string OldPic { get; set; }
    }
}
