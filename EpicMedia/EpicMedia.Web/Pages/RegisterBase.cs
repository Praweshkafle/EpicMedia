using EpicMedia.Models.Dto;
using EpicMedia.Web.Services.Interface;
using EpicMedia.Web.ViewModels;
using EpicMedia.Web.ViewModels.Validation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace EpicMedia.Web.Pages
{
    public class RegisterBase : ComponentBase
    {
        RegisterValidation validations = new RegisterValidation();
        [CascadingParameter]
        public Task<AuthenticationState> authState { get; set; }
        public RegisterVm RegisterViewModel = new RegisterVm();
        [Inject]
        public IUserService UserService { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var user = (await authState).User;
            if (user.Identity.IsAuthenticated)
            {
                navigationManager.NavigateTo("/");
            }
        }


        protected async void FormSubmit()
        {
            try
            {
                var userDto = new UserDto()
                {
                    Username = RegisterViewModel.Username,
                    Email = RegisterViewModel.Email,
                    Password = RegisterViewModel.Password
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
