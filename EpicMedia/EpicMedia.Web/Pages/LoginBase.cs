using EpicMedia.Models.Dto;
using EpicMedia.Web.Services.Implementation;
using EpicMedia.Web.Services.Interface;
using Microsoft.AspNetCore.Components;

namespace EpicMedia.Web.Pages
{
    public class LoginBase:ComponentBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        [Inject]
        public IUserService UserService { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        protected async void FormSubmit()
        {
            try
            {
                var userDto = new LoginDto()
                {
                    UserName = Username,
                    Password = Password
                };
                if (userDto == null)
                {
                    throw new Exception("Unable to Login!!!");
                }
                var result = await UserService.LoginUser(userDto);
                if (result)
                {
                    navigationManager.NavigateTo("/");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
