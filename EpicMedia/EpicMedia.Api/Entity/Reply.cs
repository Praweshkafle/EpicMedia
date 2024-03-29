﻿using MongoDB.Bson;

namespace EpicMedia.Api.Entity
{
    public class Reply
    {
        public ObjectId Id { get; set; }
        public string? Text { get; set; }
        public string? User { get; set; }
        public string postId { get; set; }
        public string ParentCommentId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
