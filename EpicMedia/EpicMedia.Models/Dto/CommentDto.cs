﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMedia.Models.Dto
{
    public class CommentDto
    {
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}