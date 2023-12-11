using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace EpicMedia.Api.Entity
{
    public class Posts
    {
        public ObjectId Id { get; set; } // Assuming ObjectId as the ID type
        public string Content { get; set; }
        public string User { get; set; }
        public string Image { get; set; }
        public List<string> Likes { get; set; } = new List<string>();
        public List<Comment> Comments { get; set; } = new List<Comment>(); // List of comments
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
