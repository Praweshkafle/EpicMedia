using EpicMedia.Web;
using EpicMedia.Web.Services.Implementation;
using EpicMedia.Web.Services.Interface;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7270/") });
builder.Services.AddScoped<IUserService, UserService>();
await builder.Build().RunAsync();
