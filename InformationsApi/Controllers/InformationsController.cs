using InformationsApi.Models;
using InformationsApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformationsApi.Controllers
{
    [Route("/informations")]
    [ApiVersionNeutral]
    [ApiController]
    public class InformationsController : ControllerBase
    {
        private readonly IInformationsService informationsService;
        public InformationsController(IInformationsService informationsService)
        {
            this.informationsService = informationsService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<InformationExtModel>>> Get(bool activeOnly = true)
        {
            return await informationsService.GetInformationsAsync(activeOnly);
        }

        [HttpGet("newest/{fromDate}")]
        public async Task<ActionResult<IEnumerable<InformationExtModel>>> GetNewest(DateTime fromDate)
        {
            return await informationsService.GetInformationsAsync(activeOnly: true, fromDate: fromDate);
        }

        [HttpGet("{informationId}")]
        public async Task<ActionResult<InformationModel>> Get(int informationId)
        {
            return await informationsService.GetInformationAsync(informationId);
        }


        [HttpPost]
        public async Task<IActionResult> Post(InformationModel information)
        {
            var errors = await informationsService.SaveInformationAsync(information);
            if (errors.Any())
            {
                var vpd = new ValidationProblemDetails
                {
                    Title = "Błąd walidacji",
                    Detail = "Nie można zapisać komunikatu",
                };
                vpd.Errors.Add("błędy walidacji", errors);

                return ValidationProblem(vpd);
            }
            return Ok();
        }

        [HttpDelete("{informationId}")]
        public async Task<IActionResult> Delete(int informationId)
        {
            await informationsService.DeleteInformationAsync(informationId);
            return Ok();
        }
    }
}
