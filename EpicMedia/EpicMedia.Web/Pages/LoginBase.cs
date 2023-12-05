using Blazored.LocalStorage;
using EpicMedia.Models.Dto;
using EpicMedia.Web.Services.Implementation;
using EpicMedia.Web.Services.Interface;
using EpicMedia.Web.Shared.Manager;
using EpicMedia.Web.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

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
        [Inject]
        public ILocalStorageService localStorageService { get; set; }

        [Inject]
        public AuthenticationStateProvider _authenticationStateProvider { get; set; }

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
                if (!string.IsNullOrEmpty(result.token?.accessToken))
                {
                   await localStorageService.SetItemAsync<string>("jwt-access-token",result.token.accessToken);
                    (_authenticationStateProvider as CustomAuthProvider)?.NotifyAuthState();
                    //navigationManager.NavigateTo("/");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
