using Blazored.LocalStorage;

namespace EpicMedia.Web.Shared.Manager
{
    public class CustomHttpHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorageService;

        public CustomHttpHandler(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.AbsolutePath.ToLower().Contains("login") ||
                request.RequestUri.AbsolutePath.ToLower().Contains("register")|| request.RequestUri.AbsolutePath.ToLower().Contains("api/posts/getallpost"))
            {

                return await base.SendAsync(request, cancellationToken);
            }

            var jwtToken = await _localStorageService.GetItemAsync<string>("jwt-access-token");

            if (!string.IsNullOrEmpty(jwtToken))
            {
                request.Headers.Clear();
                request.Headers.Add("Authorization", $"Bearer {jwtToken}");
            }
            return await base.SendAsync(request, cancellationToken);

        }
    }
}
