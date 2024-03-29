﻿using EpicMedia.Api.Entity;
using MongoDB.Bson;

namespace EpicMedia.Api.Entity
{
    public class Comment
    {
        public ObjectId Id { get; set; }
        public string? Text { get; set; }
        public string? User { get; set; }
        public List<Reply> Replies { get; set; } = new List<Reply>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
