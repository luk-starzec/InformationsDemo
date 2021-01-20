using SeasonsApi.Models;
using System.Threading.Tasks;

namespace SeasonsApi.Services
{
    public interface ISeasonsService
    {
        Task<SeasonModel[]> GetSeasonsAsync(bool activeOnly = false);
        Task<SeasonModel> GetSeasonAsync(int seasonId);
    }
}
