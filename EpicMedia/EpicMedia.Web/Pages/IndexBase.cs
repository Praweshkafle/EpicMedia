using EpicMedia.Models.Dto;
using EpicMedia.Web.Services.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Xml;

namespace EpicMedia.Web.Pages
{
    public class IndexBase:ComponentBase
    {
        [Inject]
        public AuthenticationStateProvider authProvider { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }
        public bool CreatePostDialogOpen { get; set; }
        public bool IsAuthinticate { get; set; }
        public List<CommentDto> newComments { get; set; }=new List<CommentDto>();
        public List<ReplyDto> newReply { get; set; }=new List<ReplyDto>();
        public string commentText { get; set; }
        public string UserId { get; set; }
        public List<PostDto> Posts { get; set; }
        [Inject]
        public IPostService _postService { get; set; }
        private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();

        protected override async Task OnInitializedAsync()
        {
            var authstate = await authProvider.GetAuthenticationStateAsync();
            IsAuthinticate = authstate.User.Identity.IsAuthenticated;
            Posts =await _postService.GetAllPost();
            UserId = await GetUserId();
            newComments.Clear();
            newReply.Clear();
        }


        public string selectedCommentId = null;

        public string replyText = "";

        public void ToggleReplySection(string commentId)
        {
            selectedCommentId = selectedCommentId == commentId ? null : commentId;
            replyText = ""; 
        }
        public async Task PostReply(string postId, string commentId)
        {
            var reply = new ReplyDto
            {
                CreatedAt = DateTime.Now,
                ParentCommentId = commentId,
                postId = postId,
                Text = replyText,
                User = UserId,
            };

            newReply.Add(reply);

            var response = await _postService.PostCommentReplyAsync(reply);
            if (response.Success)
            {
                replyText = "";
                StateHasChanged();
            }
            selectedCommentId = null;
        }


        public string GetLikeIcon(string postId)
        {
            var post = Posts.FirstOrDefault(p => p.Id == postId);
            if (post != null && post.Likes.Contains(UserId))
            {
                return "fa fa-heart";
            }
            return "fa fa-heart-o";
        }

        public async void ToggleLike(PostDto post)
        {
            if (post.Likes.Contains(UserId))
            {
                post.Likes.Remove(UserId);
            }
            else
            {
                post.Likes.Add(UserId);
            }
            var result =await _postService.LikeDislikeAsync(post.Id);
        }

        async Task<string> GetUserId()
        {
            var authState = await authProvider
                 .GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                claims = user.Claims.ToList();
                var userid = claims.Where(a => a.Type == "Id").Select(b => b.Value).FirstOrDefault();
                return userid;
            }
            return "";
        }
        public async Task PostComment(string postId)
        {
            try
            {
                if (!IsAuthinticate)
                {
                    navigationManager.NavigateTo("/login");
                }
                string userId = await GetUserId();
                newComments.Add(new CommentDto
                {
                    User = userId,
                    CreatedAt = DateTime.Now,
                    Text = commentText,
                    postId=postId,
                    Replies=new List<ReplyDto>(),
                });
                var commentDto = new CommentDto
                {
                    Text = commentText,
                    CreatedAt = DateTime.Now,
                    User = userId
                };
                var response = await _postService.PostCommentAsync(commentDto,postId);
                if (response.Success)
                {
                    commentText = "";
                    StateHasChanged();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void OnCreatePostDialogClose(bool accepted)
        {
            CreatePostDialogOpen = false;
            StateHasChanged();
        }

        public void OpenCreatePostDialog()
        {
            CreatePostDialogOpen = true;
            StateHasChanged();
        }
    }
}
