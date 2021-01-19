using SeasonsApi.Models;

namespace SeasonsApi.Services
{
    public interface ISeasonsService
    {
        SeasonModel[] GetSeasons(bool activeOnly = false);
        SeasonModel GetSeason(int seasonId);
    }
}
