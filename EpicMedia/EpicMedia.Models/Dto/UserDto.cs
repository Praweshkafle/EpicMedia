using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMedia.Models.Dto
{
    public class UserDto
    {
        public ObjectId Id { get; set; }
        [Required(ErrorMessage = "Username is mandatory")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is mandatory")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is mandatory")]
        public string Password { get; set; }

        public string ProfilePicture { get; set; } = "";
    }
}
