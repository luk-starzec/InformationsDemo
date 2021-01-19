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
        private readonly string apiUrl = "http://zergling:8502";
        private readonly string apiVersion = "1.1";

        private readonly IHttpClientFactory _clientFactory;
        public InformationsService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public Task<NewestInformation[]> GetNewestInformationsAsync()
        {
            var informations = new List<NewestInformation>();
            for (int i = 1; i < 10; i++)
                informations.Add(GetTestNewestInformation(i));

            informations[1].Seasons = "Lato 2020";
            informations[1].CategoryName = "Hiszpania";
            informations[3].Seasons = "Lato 2020, Zima 2020/21";
            informations[3].CategoryName = "Hiszpania - Costa Brava";

            return Task.FromResult(informations.ToArray());
        }

        public async Task<Category[]> GetCategoriesAsync()
        {
            //return new Category[0];

            var informations = await GetInformations();

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

        private async Task<InformationExtModel[]> GetInformations()
        {
            var informationsClient = new Client(apiUrl, _clientFactory.CreateClient());

            var komunikaty = await informationsClient.Informations_Get_activeAsync(active: true, apiVersion);

            return komunikaty.ToArray();
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

        public async Task<Category[]> GetCategoriesAsync2()
        {
            var categories = new List<Category>();
            for (int i = 1; i < 5; i++)
                categories.Add(GetTestCategory(i));
            categories[2].Type = CategoryType.Destination;
            categories[3].Type = CategoryType.Destination;

            var subCategories = new List<Category>();
            for (int i = 10; i < 12; i++)
            {
                var subCategory = GetTestCategory(i);
                subCategory.Type = CategoryType.SubDestination;
                subCategories.Add(subCategory);
            }
            categories[3].SubCategories = subCategories.ToArray();

            return categories.ToArray();
        }



        private Information GetTestInformation(int index)
        {
            return new Information
            {
                Title = $"Testowy komunikat numer {index}",
                Url = "https://images.r.pl/pliki/20611039321073.pdf",
                Date = new DateTime(2020, 6, 3).AddDays(index),
            };
        }
        private NewestInformation GetTestNewestInformation(int index)
        {
            return new NewestInformation
            {
                Title = $"Testowy komunikat> numer {index}",
                Url = "https://images.r.pl/pliki/20611039321073.pdf",
                Date = new DateTime(2020, 6, 3).AddDays(index),
                CategoryName = "Test",
            };
        }

        private Category GetTestCategory(int index = 1)
        {
            var informations = new List<Information>();
            for (int i = 1; i < 10; i++)
                informations.Add(GetTestInformation(i));

            if (index != 1)
            {
                informations[1].Seasons = "Lato 2020";
                informations[3].Seasons = "Lato 2020, Zima 2020/21";
            }


            return new Category
            {
                Title = $"Przykładowa kategoria numer {index}",
                Type = CategoryType.General,
                Informations = informations.ToArray(),
                Order = index,
            };
        }
    }
}
