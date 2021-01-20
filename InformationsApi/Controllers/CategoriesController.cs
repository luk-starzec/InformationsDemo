using InformationsApi.ApiVersionAttributes;
using InformationsApi.Models;
using InformationsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformationsApi.Controllers
{
    [Route("/categories")]
    //[ApiVersionNeutral]
    [ApiVersion("1.0", Deprecated = true)]
    //[ApiVersion("1.1"), ApiVersion("2.0")]
    //[ApiVersionRangeAttribute(1, 3)]
    [ApiVersionCurrent()]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly IInformationsService informationsService;
        private readonly ILogger<CategoriesController> logger;

        public CategoriesController(IInformationsService informationsService, ILogger<CategoriesController> logger)
        {
            this.informationsService = informationsService;
            this.logger = logger;
        }


        [HttpGet]
        //[Obsolete]
        //[MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> Get()
        {
            return await informationsService.GetCategoriesAsync();
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<CategoryModel>> Get(int categoryId)
        {
            return await informationsService.GetCategoryAsync(categoryId);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryModel category)
        {
            var errors = await informationsService.SaveCategoryAsync(category);
            if (errors.Any())
            {
                var vpd = new ValidationProblemDetails
                {
                    Title = "Błąd walidacji",
                    Detail = "Nie można zapisać kategorii",
                };
                vpd.Errors.Add("błędy walidacji", errors);

                return ValidationProblem(vpd);
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(IEnumerable<CategoryModel> categories)
        {
            if (categories.Any(r => r.CategoryId == 0))
                return BadRequest("KategoriaId musi być większe od 0");

            var errors = await informationsService.SaveCategoriesAsync(categories.ToArray());
            if (errors.Any())
            {
                var vpd = new ValidationProblemDetails
                {
                    Title = "Błąd walidacji",
                    Detail = "Nie można zapisać kategorii",
                };
                foreach (var blad in errors)
                    vpd.Errors.Add(blad);

                return ValidationProblem(vpd);
            }
            return Ok();
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var errors = await informationsService.DeleteCategoryAsync(categoryId);
            if (errors.Any())
            {
                var vpd = new ValidationProblemDetails
                {
                    Title = "Błąd walidacji",
                    Detail = "Nie można usunąć kategorii komunikatów",
                };
                vpd.Errors.Add("błędy walidacji", errors);

                return ValidationProblem(vpd);
            }
            return Ok();
        }
    }
}