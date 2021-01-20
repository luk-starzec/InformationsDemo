using Microsoft.AspNetCore.Mvc;
using SeasonsApi.Models;
using SeasonsApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeasonsApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("/seasons")]
    [ApiController]
    public class SeasonsController : ControllerBase
    {
        private readonly ISeasonsService seasonsService;
        public SeasonsController(ISeasonsService seasonsService)
        {
            this.seasonsService = seasonsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeasonModel>>> Get(bool activeOnly = false)
        {
            return await seasonsService.GetSeasonsAsync(activeOnly);
        }

        [HttpGet("{seasonId}")]
        public async Task<ActionResult<SeasonModel>> Get(int seasonId)
        {
            return await seasonsService.GetSeasonAsync(seasonId);
        }
    }
}
