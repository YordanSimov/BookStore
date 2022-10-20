using Microsoft.Extensions.Options;
using ProjectDK.BL.Interfaces;
using ProjectDK.Models.Configurations;
using ProjectDK.Models.Models;

namespace ProjectDK.BL.Services
{
    public class AdditionalInfoProvider : IAdditionalInfoProvider
    {
        private readonly IOptionsMonitor<HttpClientSettings> settings;
        private readonly HttpClient httpClient;
        private readonly string Url;

        public AdditionalInfoProvider(HttpClient httpClient, IOptionsMonitor<HttpClientSettings> settings)
        {
            this.httpClient = httpClient;
            this.settings = settings;
            Url = settings.CurrentValue.AdditionalInfoUrl;
        }

        public async Task<string> GetAdditionalInfo(Book book)
        {
            var response = await httpClient.GetAsync($"{Url}{book.AuthorId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}