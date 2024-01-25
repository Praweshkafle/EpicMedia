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
                //var post =await _postRepository.GetById(postId);
                //if (post == null) { return BadRequest(new { Sucess = false, Message = "Error occured!" }); }

                //var comment = new Comment
                //{
                //    CreatedAt = DateTime.Now,
                //    Text = commentDto.Text,
                //    User = post.User,
                //};

                //post.Comments.Add(comment);

                //var result = await _postRepository.Update(postId,post);

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
                var postDtos = posts.Select(post => new PostDto
                {
                    Id = post.Id.ToString(), 
                    Content = post.Content,
                    User = post.User,
                    Image = imgurl+post.Image,
                    Likes = post.Likes,
                    CreatedAt = post.CreatedAt
                }).ToList();

                return Ok(postDtos);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string GetPostImageFullPath()
        {
            return "https://localhost:7270/Images/Custom/";
        }
    }
}
