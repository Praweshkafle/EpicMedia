using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMedia.Models.Dto
{
    public class CommentDto
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public string postId { get; set; }
        public List<ReplyDto> Replies { get; set; } = new List<ReplyDto>();
        public DateTime CreatedAt { get; set; } = DateTime.Now; 
    }
}
