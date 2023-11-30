using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace EpicMedia.Api.Entity
{
    public class Posts
    {
        public ObjectId Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
        public string Image { get; set; }

        public DateTime PublicationDate { get; set; }

        public int AuthorId { get; set; }

    }
}
