using Blazored.LocalStorage;
using Blazored.Modal;
using EpicMedia.Web;
using EpicMedia.Web.Services.Implementation;
using EpicMedia.Web.Services.Interface;
using EpicMedia.Web.Shared.Manager;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7270/") });

builder.Services.AddHttpClient("EpicMediaApi", options =>
{
    options.BaseAddress = new Uri("https://localhost:7270/");
}).AddHttpMessageHandler<CustomHttpHandler>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredModal();
builder.Services.AddScoped<CustomHttpHandler>();
await builder.Build().RunAsync();
