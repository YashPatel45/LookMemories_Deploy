using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LookMemories_WEB.Model.DataBase
{
    public class Album
    {
        public  int Id { get; set; }

        public string AlbumName { get; set; }

        public string CreatedDate { get; set; }

        public virtual AccountUser AccountUser { get; set; }
        public string AccountUserId { get; set; }

        public List<Photos> PhotoList { get; set; }

        [NotMapped]
        public List<IFormFile> FileLists { get; set; }

        [NotMapped]
        public string ProtectorId { get; set; }

      

    }
}
