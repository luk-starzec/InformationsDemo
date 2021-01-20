using SeasonsApiClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace InformationsApi.Services
{
    public class SeasonsService : ISeasonsService
    {
        private static readonly HttpClient httpClient = new HttpClient();

        private readonly string seasonsApiUrl;
        private readonly string apiVersion = "1.0";
        public SeasonsService(string seasonsApiUrl)
        {
            this.seasonsApiUrl = seasonsApiUrl;

            var version = typeof(Client).Assembly.GetName().Version;
            apiVersion = $"{version.Major}.{version.Minor}";
        }

        public Task<ICollection<SeasonModel>> GetSeasons()
        {
            var seasonsClient = new Client(seasonsApiUrl, httpClient);

            return seasonsClient.Seasons_Get_activeOnlyAsync(activeOnly: false, x_Version: apiVersion);
        }
    }
}
