using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EpicMedia.Models.Dto
{
    public class PostDto
    {
        public string Id { get; set; } // Assuming ObjectId as the ID type
        public string Content { get; set; }
        public string User { get; set; }
        public string Image { get; set; }
        public List<string> Likes { get; set; } = new List<string>();
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>(); // List of comments
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
