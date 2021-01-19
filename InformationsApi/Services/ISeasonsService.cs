using SeasonsApiClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InformationsApi.Services
{
    public interface ISeasonsService
    {
        Task<ICollection<SeasonModel>> GetSeasons();
    }
}
