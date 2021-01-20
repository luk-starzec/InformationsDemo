using InformationsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InformationsApi.Repo
{
    public static class FakeData
    {
        public static string GetFileUrl() => "data/test.pdf";

        public static CategoryModel[] GetCategories()
        {
            return new CategoryModel[]
            {
                new CategoryModel
                {
                    CategoryId = 1,
                    Name = "Ogólne",
                    Order = 1,
                },
                new CategoryModel
                {
                    CategoryId = 2,
                    Name = "Ubezpieczenia",
                    Order = 2,
                },
                new CategoryModel
                {
                    CategoryId = 3,
                    Name = "Promocje",
                    Order = 3,
                },
                new CategoryModel
                {
                    CategoryId = 4,
                    Name = "Bułgaria",
                    IsDestination = true,
                    Order = 4,
                },
                new CategoryModel
                {
                    CategoryId = 5,
                    Name = "Egipt",
                    IsDestination = true,
                    Order = 5,
                },
                new CategoryModel
                {
                    CategoryId = 6,
                    Name = "Hiszpania",
                    IsDestination = true,
                    Order = 6,
                },
                new CategoryModel
                {
                    CategoryId = 7,
                    ParentCategoryId = 6,
                    Name = "Costa Brava",
                    IsDestination = true,
                    Order = 1,
                },
                new CategoryModel
                {
                    CategoryId = 8,
                    ParentCategoryId = 6,
                    Name = "Costa Almeria",
                    IsDestination = true,
                    Order = 2,
                },
                new CategoryModel
                {
                    CategoryId = 9,
                    ParentCategoryId = 6,
                    Name = "Costa del Sol",
                    IsDestination = true,
                    Order = 3,
                },
                new CategoryModel
                {
                    CategoryId = 10,
                    Name = "Kenia",
                    IsDestination = true,
                    Order = 7,
                },
                new CategoryModel
                {
                    CategoryId = 11,
                    Name = "Maroko",
                    IsDestination = true,
                    Order = 8,
                },
                new CategoryModel
                {
                    CategoryId = 12,
                    Name = "Turcja",
                    IsDestination = true,
                    Order = 9,
                },
            };
        }


        public static InformationModel[] GetInformations()
        {
            var result = new List<InformationModel>();

            var categories = GetCategories();
            int ci = 0;
            for (int i = 1; i <= 50; i++)
            {
                if (ci == categories.Count() - 1)
                    ci = 0;
                var categoryId = categories[ci++].CategoryId;
                result.Add(GetInformation(i, categoryId));
            }

            return result.ToArray();
        }

        private static InformationModel GetInformation(int informationId, int categoryId)
        {
            var result = new InformationModel
            {
                InformationId = informationId,
                Title = $"{ (informationId % 2 == 0 ? "Testowy komuniakt" : "Przykładowa informacja")} {informationId}",
                CategoryId = categoryId,
                FileUrl = GetFileUrl(),
            };

            if (informationId % 7 == 0)
            {
                result.ValidFrom = DateTime.Today.AddMonths(-2);
                result.ValidTo = DateTime.Today.AddMonths(-1);
            }
            else
            {
                result.ValidFrom = DateTime.Today.AddDays(-15 + informationId % 10);
                result.ValidTo = result.ValidFrom.AddMonths(2);
            }
            result.Added = result.ValidFrom;

            if (informationId % 3 == 0)
                result.SeasonsId = new int[] { 10, 12 };
            else if (informationId % 5 == 0)
                result.SeasonsId = new int[] { 11 };
            else if (informationId % 6 == 0)
                result.SeasonsId = new int[] { 12, 13 };
            else if (informationId % 8 == 0)
                result.SeasonsId = new int[] { 11, 12, 13 };

            return result;
        }

    }
}
