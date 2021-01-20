using SeasonsApi.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SeasonsApi.Services
{
    public class SeasonsService : ISeasonsService
    {
        private readonly SeasonModel[] fakeData;
        public SeasonsService()
        {
            fakeData = InitFakeData();
        }

        public async Task<SeasonModel[]> GetSeasonsAsync(bool activeOnly = false)
        {
            var query = fakeData.Select(r => r).AsQueryable();

            if (activeOnly)
                query = query.Where(r => r.IsActive);

            var result = query.ToArray();

            return await Task.FromResult(result);
        }

        public async Task<SeasonModel> GetSeasonAsync(int seasonId)
        {
            var result = fakeData.SingleOrDefault(r => r.SeasonId == seasonId);

            return await Task.FromResult(result);
        }


        private SeasonModel[] InitFakeData()
        {
            return new SeasonModel[]
            {
                new SeasonModel
                {
                    SeasonId = 10,
                    Name="Lato 2020",
                    Order=1,
                },
                new SeasonModel
                {
                    SeasonId = 11,
                    Name="Zima 2020/21",
                    Order=2,
                },
                new SeasonModel
                {
                    SeasonId = 12,
                    Name="Lato 2021",
                    Order=3,
                },
                new SeasonModel
                {
                    SeasonId = 13,
                    Name="Zima 2021/22",
                    Order=4,
                },
            };
        }
    }
}
