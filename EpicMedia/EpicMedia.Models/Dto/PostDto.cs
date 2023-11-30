using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMedia.Models.Dto
{
    public class PostDto
    {
        public ObjectId Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
        public string Image { get; set; }

        public DateTime PublicationDate { get; set; }

        public int AuthorId { get; set; }
    }
}
