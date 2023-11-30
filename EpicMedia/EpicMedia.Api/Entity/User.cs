using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace EpicMedia.Api.Entity
{
    public class User
    {
        public ObjectId Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; } 

        public string ProfilePicture { get; set; } = "";
    }
}
