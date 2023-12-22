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
    public class LoginBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> authState { get; set; }
        public LoginViewModel loginViewModel = new LoginViewModel();
        [Inject]
        public IUserService UserService { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }
        [Inject]
        public ILocalStorageService localStorageService { get; set; }

        [Inject]
        public AuthenticationStateProvider _authenticationStateProvider { get; set; }

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
                var userDto = new LoginDto()
                {
                    UserName = loginViewModel.Username,
                    Password = loginViewModel.Password,
                };
                if (userDto == null)
                {
                    throw new Exception("Unable to Login!!!");
                }
                var result = await UserService.LoginUser(userDto);
                if (!string.IsNullOrEmpty(result.jwtTokenResponse?.accessToken))
                {
                    await localStorageService.SetItemAsync<string>("jwt-access-token", result.jwtTokenResponse.accessToken);
                    (_authenticationStateProvider as CustomAuthProvider)?.NotifyAuthState();
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
