using EpicMedia.Api.Entity;
using EpicMedia.Api.Filemanager;
using EpicMedia.Api.Repository.Interface;
using EpicMedia.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using System.Linq;
using System.Security.Claims;

namespace EpicMedia.Api.Controllers
{
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly IFileManager _fileManager;
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository ,IFileManager fileManager)
        {
            _postRepository = postRepository;
            _fileManager = fileManager;
        }

        [HttpPost]
        [Route("create")]
        [Authorize]
        public async Task<IActionResult> create([FromForm] string postdto, [FromForm] IFormFile file)
        {
            try
            {
                PostDto postDto =  Newtonsoft.Json.JsonConvert.DeserializeObject<PostDto>(postdto);
                if(postDto == null) { return BadRequest(new { Sucess = false, Message = "Error occured!" }); }
                string imagePath="";
                if (file != null)
                {
                    var prefix = Guid.NewGuid();
                    imagePath = _fileManager.saveImageAndGetFileName(file, prefix.ToString());
                }
                var post = new Posts
                {
                    Content = postDto.Content,
                    CreatedAt = DateTime.Now,
                    Image = imagePath,
                    Likes = postDto.Likes,
                    User = postDto.User,
                    Comments = null
                };
               
                await _postRepository.Create(post);

                return Ok(new {Sucess=true,Message="Post created successfully!"});
            }
            catch (Exception)
            {
                return BadRequest(new { Sucess = false, Message = "Error occured!!" });
            }
        }

        [HttpPost]
        [Route("comment/{postId}")]
        public async Task<IActionResult> comment([FromBody] CommentDto commentDto,string postId)
        {
            try
            {
                var post = await _postRepository.GetById(new ObjectId(postId));
                if (post == null) { return BadRequest(new { Sucess = false, Message = "Error occured!" }); }
                if (post.Comments == null)
                {
                    post.Comments = new List<Comment>();
                }
                var comment = new Comment
                {
                    Id = ObjectId.GenerateNewId(),
                    CreatedAt = DateTime.Now,
                    Text = commentDto.Text,
                    User = post.User,
                };

                post.Comments.Add(comment);

                var result = await _postRepository.Update(new ObjectId(postId), post);

                return Ok(new { Sucess = true, Message = "Post updated successfully!" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("getallpost")]
        public async Task<IActionResult> getallpost()
        {
            try
            {
                var posts = await _postRepository.GetAll();
                if (posts == null) return NotFound();
                string imgurl = GetPostImageFullPath();
                
                var postDtos = posts.ToList().Select( post => new PostDto
                {
                    Id = post.Id.ToString(), 
                    Content = post.Content,
                    User = post.User,
                    Image = imgurl+post.Image,
                    Likes = post.Likes,
                    CreatedAt = post.CreatedAt,
                    Comments=  convertToDto(post.Comments),
                }).ToList();

                return Ok(postDtos);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost("like")]
        [Authorize] // Ensure the user is authenticated
        public async Task<IActionResult> LikePost([FromBody]  string postId)
        {
            try
            {
                string userId = User.FindFirst("Id")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var post = await _postRepository.GetById(new ObjectId(postId));

                if (post != null)
                {
                    if (post.Likes.Contains(userId))
                    {
                        post.Likes.Remove(userId);
                    }
                    else
                    {
                        post.Likes.Add(userId);
                    }

                    await _postRepository.Update(new ObjectId(postId), post);

                    return Ok(new { Success = true, Message = "Post liked successfully!" });
                }

                return BadRequest(new { Success = false, Message = "Post not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = "Error occurred while liking the post.", Error = ex.Message });
            }
        }


        private string GetPostImageFullPath()
        {
            return "https://localhost:7270/Images/Custom/";
        }

        private List<CommentDto> convertToDto(List<Comment> comments)
        {
            if (comments == null)
            {
                return new List<CommentDto>();
            }
            var result = new List<CommentDto>();
            foreach (var comment in comments)
            {
                result.Add(new CommentDto
                {
                    Id = comment.Id.ToString(),
                    CreatedAt = comment.CreatedAt,
                    Text = comment.Text,
                    User = comment.User,
                });
            }
            return result;
        }
    }
}
