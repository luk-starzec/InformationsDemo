using InformationsBlazor.Models;
using KomunikatyGlobalne.Klient;
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

            var komunikaty = await ListaKomunikatow();

            var kategorie = komunikaty.GroupBy(r => r.Kategoria.KategoriaId).Select(r => r.First().Kategoria).ToArray();

            var categories = new List<Category>();
            foreach (var kategoria in kategorie)
            {
                var kategoriaId = kategoria.KategoriaId ?? 0;

                var podkategorie = komunikaty
                    .Where(r => r.Kategoria.KategoriaId == kategoriaId)
                    .Where(r => r.Podkategoria != null)
                    .GroupBy(r => r.Podkategoria.KategoriaId)
                    .Select(r => r.First().Podkategoria)
                    .ToArray();

                var subCategories = podkategorie
                    .Select(r => new Category
                    {
                        Title = r.Nazwa,
                        Type = r.CzyDestynacja == true ? CategoryType.SubDestination : CategoryType.General,
                        Order = r.Kolejnosc ?? 0,
                        Informations = GetInformations(komunikaty, kategoriaId, r.KategoriaId ?? 0),
                    }).ToArray();

                var category = new Category
                {
                    Title = kategoria.Nazwa,
                    Type = kategoria.CzyDestynacja == true ? CategoryType.Destination : CategoryType.General,
                    Order = kategoria.Kolejnosc ?? 0,
                    Informations = GetInformations(komunikaty, kategoriaId),
                    SubCategories = subCategories,
                };
                categories.Add(category);
            }
            return categories.ToArray();
        }


        private async Task<KomunikatDoListyModel[]> ListaKomunikatow()
        {
            var komunikatyKlient = new Client(apiUrl, _clientFactory.CreateClient());

            var komunikaty = await komunikatyKlient.KomunikatyGetAsync(tylkoAktywne: true, apiVersion);

            return komunikaty.ToArray();
        }


        private Information[] GetInformations(KomunikatDoListyModel[] komunikaty, int kategoriaId, int podkategoriaId = 0)
        {
            return komunikaty
                .Where(r => r.Kategoria.KategoriaId == kategoriaId)
                .Where(r => (r.Podkategoria?.KategoriaId ?? 0) == podkategoriaId)
                .Select(r => new Information
                {
                    Title = r.Tytul,
                    Url = r.LinkDoPliku,
                    Date = r.DataDodania ?? new DateTime(2000, 1, 1),
                    Seasons = r.SezonyId.Any() ? r.SezonyJakoTekst : string.Empty,
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
