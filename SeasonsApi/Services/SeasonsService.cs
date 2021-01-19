using SeasonsApi.Models;
using System.Linq;

namespace SeasonsApi.Services
{
    public class SeasonsService : ISeasonsService
    {
        private readonly SeasonModel[] fakeData;
        public SeasonsService()
        {
            fakeData = InitFakeData();
        }

        public SeasonModel[] GetSeasons(bool activeOnly = false)
        {
            var query = fakeData.Select(r => r).AsQueryable();

            if (activeOnly)
                query = query.Where(r => r.IsActive);

            return query.ToArray();
        }

        public SeasonModel GetSeason(int seasonId)
        {
            return fakeData.SingleOrDefault(r => r.SeasonId == seasonId);
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
