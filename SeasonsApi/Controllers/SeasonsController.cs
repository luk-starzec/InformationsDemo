using Microsoft.AspNetCore.Mvc;
using SeasonsApi.Models;
using SeasonsApi.Services;
using System.Collections.Generic;

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
        public ActionResult<IEnumerable<SeasonModel>> Get(bool active = false)
        {
            return seasonsService.GetSeasons(active);
        }

        [HttpGet("{seasonId}")]
        public ActionResult<SeasonModel> Get(int seasonId)
        {
            return seasonsService.GetSeason(seasonId);
        }
    }
}
