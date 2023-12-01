using EpicMedia.Models.Dto;
using EpicMedia.Web.Services.Interface;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace EpicMedia.Web.Pages
{
    public class RegisterBase : ComponentBase
    {

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePicture { get; set; } = "";
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
                var userDto = new UserDto()
                {
                    Username = Username,
                    Email = Email,
                    Password = Password
                };
                if (userDto == null)
                {
                    throw new Exception("Unable to register!!!");
                }
                var result = await UserService.AddUser(userDto);
                if (result)
                {
                    navigationManager.NavigateTo("/login");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
