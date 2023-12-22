using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace EpicMedia.Web.Pages
{
    public class IndexBase:ComponentBase
    {
        public bool CreatePostDialogOpen { get; set; }

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
