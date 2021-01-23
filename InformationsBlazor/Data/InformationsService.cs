using InformationsBlazor.Models;
using InformationsApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace InformationsBlazor.Data
{
    public class InformationsService
    {
        //private readonly string apiUrl = "https://localhost:44392/";
        private readonly string apiUrl = "http://informationsapi/";
        private readonly string apiVersion = "1.0";

        private readonly IHttpClientFactory _clientFactory;
        public InformationsService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<NewestInformation[]> GetNewestInformationsAsync()
        {
            var informationsClient = new Client(apiUrl, _clientFactory.CreateClient());

            var fromDate = DateTime.Today.AddDays(-7);
            var models = await informationsClient.Informations_GetNewest_fromDateAsync(fromDate, apiVersion);

            var informations = models
                .Select(r => new NewestInformation
                {
                    Title = r.Title,
                    CategoryName = $"{r.Category.Name}{(r.Subcategory != null ? " - " + r.Subcategory.Name : string.Empty)}",
                    Url = r.FileUrl,
                    Date = r.Added,
                    Seasons = r.SeasonIds.Any() ? r.SeasonsText : string.Empty,
                }).ToArray();

            return informations;
        }

        public async Task<Category[]> GetCategoriesAsync()
        {
            var informations = await GetInformationExtModels();

            var categories = informations.GroupBy(r => r.Category.CategoryId).Select(r => r.First().Category).ToArray();

            var result = new List<Category>();
            foreach (var category in categories)
            {
                var subcategories = informations
                    .Where(r => r.Category.CategoryId == category.CategoryId)
                    .Where(r => r.Subcategory != null)
                    .GroupBy(r => r.Subcategory.CategoryId)
                    .Select(r => r.First().Category)
                    .ToArray();

                var subItems = subcategories
                    .Select(r => new Category
                    {
                        Title = r.Name,
                        Type = r.IsDestination ? CategoryType.SubDestination : CategoryType.General,
                        Order = r.Order,
                        Informations = GetInformations(informations, category.CategoryId, r.CategoryId),
                    }).ToArray();

                var item = new Category
                {
                    Title = category.Name,
                    Type = category.IsDestination ? CategoryType.Destination : CategoryType.General,
                    Order = category.Order,
                    Informations = GetInformations(informations, category.CategoryId),
                    SubCategories = subItems,
                };
                result.Add(item);
            }
            return result.ToArray();
        }

        private async Task<InformationExtModel[]> GetInformationExtModels()
        {
            var informationsClient = new Client(apiUrl, _clientFactory.CreateClient());

            var informations = await informationsClient.Informations_Get_activeOnlyAsync(activeOnly: true, apiVersion);

            return informations.ToArray();
        }


        private Information[] GetInformations(InformationExtModel[] informations, int categoryId, int subCategoryId = 0)
        {
            return informations
                .Where(r => r.Category.CategoryId == categoryId)
                .Where(r => (r.Subcategory?.CategoryId ?? 0) == subCategoryId)
                .Select(r => new Information
                {
                    Title = r.Title,
                    Url = r.FileUrl,
                    Date = r.Added,
                    Seasons = r.SeasonIds.Any() ? r.SeasonsText : string.Empty,
                }).ToArray();
        }
    }
}
