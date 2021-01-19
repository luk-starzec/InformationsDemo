using InformationsApi.Models;
using InformationsApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public ActionResult<IEnumerable<InformationExtModel>> Get(bool active = true)
        {
            return informationsService.GetInformations(active);
        }

        [HttpGet("newest/{fromDate}")]
        public ActionResult<IEnumerable<InformationExtModel>> GetNajnowsze(DateTime fromDate)
        {
            return informationsService.GetInformations(activeOnly: true, fromDate: fromDate);
        }

        [HttpGet("{informationId}")]
        public ActionResult<InformationModel> Get(int informationId)
        {
            return informationsService.GetInformation(informationId);
        }


        [HttpPost]
        public IActionResult Post(InformationModel information)
        {
            var errors = informationsService.SaveInformation(information);
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
        public IActionResult Delete(int informationId)
        {
            informationsService.DeleteInformation(informationId);
            return Ok();
        }
    }
}
