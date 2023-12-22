using EpicMedia.Api.Entity;
using EpicMedia.Api.Repository.Interface;
using EpicMedia.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace EpicMedia.Api.Controllers
{
    [Authorize]
    [Route("api/post")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> create(PostDto postDto)
        {
            try
            {
                if(postDto == null) { return BadRequest(new { Sucess = false, Message = "Error occured!" }); }

                var post = new Posts
                {
                    Content = postDto.Content,
                    CreatedAt = DateTime.Now,
                    Image = postDto.Image,
                    Likes = postDto.Likes,
                    User = postDto.User,
                    Comments = null
                };
                await _postRepository.Create(post);

                return Ok(new {Sucess=true,Message="Post created successfully!"});
            }
            catch (Exception)
            {

                throw;
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
    }
}
