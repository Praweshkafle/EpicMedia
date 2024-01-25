using Blazored.Modal.Services;
using EpicMedia.Models.Dto;
using EpicMedia.Web.Services.Interface;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace EpicMedia.Web.Pages
{
    public class IndexBase:ComponentBase
    {
        public bool CreatePostDialogOpen { get; set; }
        public List<PostDto> Posts { get; set; }
        [Inject]
        public IPostService _postService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Posts =await _postService.GetAllPost();
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
