using InformationsApi.Models;
using InformationsApi.Repo;
using SeasonsApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformationsApi.Services
{
    public class InformationsService : IInformationsService
    {
        private readonly IInformationsRepo informationsRepo;
        private readonly ISeasonsService seasonsService;

        public InformationsService(IInformationsRepo informationsRepo, ISeasonsService seasonsService)
        {
            this.informationsRepo = informationsRepo;
            this.seasonsService = seasonsService;
        }


        public async Task<InformationExtModel[]> GetInformationsAsync(bool activeOnly, DateTime? fromDate = null)
        {
            var informations = await informationsRepo.GetInformationsAsync(activeOnly, fromDate);

            var categoryIds = informations.Select(r => r.CategoryId).Distinct().ToArray();
            var categories = await informationsRepo.GetCategoriesAsync(categoryIds);

            var seasons = await seasonsService.GetSeasons();

            return informations
                .Select(r => Information2InformationExt(r, categories, seasons.ToArray()))
                .OrderBy(r => r.Category.Order)
                .ThenBy(r => r.Subcategory != null ? r.Subcategory.Order : 0)
                .ToArray();
        }

        private InformationExtModel Information2InformationExt(InformationModel information, CategoryModel[] categories, SeasonModel[] seasons)
        {
            var informationCategory = categories.Single(r => r.CategoryId == information.CategoryId);
            var category = informationCategory.ParentCategoryId.HasValue ? categories.Single(r => r.CategoryId == informationCategory.ParentCategoryId) : informationCategory;
            var subcategory = informationCategory.ParentCategoryId.HasValue ? informationCategory : null;

            var seasonsText = GetSeasonsText(information.SeasonsId, seasons);

            return new InformationExtModel
            {
                InformationId = information.InformationId,
                Category = category,
                Subcategory = subcategory,
                Title = information.Title,
                ValidFrom = information.ValidFrom,
                ValidTo = information.ValidTo,
                Added = information.Added,
                FileUrl = information.FileUrl,
                SeasonIds = information.SeasonsId,
                SeasonsText = seasonsText,
            };
        }

        private string GetSeasonsText(int[] seasonIds, SeasonModel[] seasons)
        {
            var names = seasons
                .Where(r => seasonIds.Contains(r.SeasonId))
                .OrderBy(r => r.Order)
                .Select(r => r.Name)
                .ToArray();

            return names.Any()
                ? string.Join(", ", names)
                : "wszystkie";
        }

        public async Task<InformationModel> GetInformationAsync(int informationId)
        {
            return await informationsRepo.GetInformationAsync(informationId);
        }


        public async Task<string[]> SaveInformationAsync(InformationModel information)
        {
            var errors = ValidateInformation(information);
            if (errors.Any())
                return errors;

            if (information.InformationId == 0)
            {
                var fileUrl = await informationsRepo.UploadFileAsync(information.FileUrl, information.File);

                if (string.IsNullOrEmpty(fileUrl))
                    return new string[] { "Wystąpił błąd przy zapisie pliku na serwer" };

                information.FileUrl = fileUrl;
            }

            await informationsRepo.SaveInformationAsync(information);

            return new string[0];
        }

        private string[] ValidateInformation(InformationModel information)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(information.Title))
                errors.Add("Tytuł nie może być pusty");

            if (information.CategoryId == 0)
                errors.Add("Wybierz kategorię komunikatu");

            if (string.IsNullOrWhiteSpace(information.FileUrl))
                errors.Add("Wybierz plik komunikatu");
            else if (information.InformationId == 0 && (information.File == null || information.File.Length == 0))
                errors.Add("Wybierz plik komunikatu");

            return errors.ToArray();
        }

        public async Task DeleteInformationAsync(int informationId)
        {
            await informationsRepo.DeleteInformationAsync(informationId);
        }


        public async Task<CategoryModel[]> GetCategoriesAsync()
        {
            return await informationsRepo.GetCategoriesAsync();
        }

        public async Task<CategoryModel> GetCategoryAsync(int categoryId)
        {
            return await informationsRepo.GetCategoryAsync(categoryId);
        }

        public async Task<string[]> SaveCategoryAsync(CategoryModel category)
        {
            var errors = await SaveCategoriesAsync(new CategoryModel[] { category });

            if (errors.Any())
                return errors.First().Value;

            return new string[0];
        }

        public async Task<IDictionary<string, string[]>> SaveCategoriesAsync(CategoryModel[] categories)
        {
            var errors = new Dictionary<string, string[]>();
            foreach (var category in categories)
            {
                var categoryErrors = ValidateCategory(category);
                if (categoryErrors.Any())
                    errors.Add($"Kategoria id: {category.CategoryId}", categoryErrors);
            }
            if (errors.Any())
                return errors;

            await informationsRepo.SaveCategoriesAsync(categories);

            return new Dictionary<string, string[]>();
        }

        private string[] ValidateCategory(CategoryModel category)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(category.Name))
                errors.Add("Nazwa kategorii nie może być pusta");

            if (category.Order < 1)
                errors.Add("Kolejność powinna być większa od 0");

            if (category.Order >= 1000)
                errors.Add("Kolejność powinna być mniejsza od 1000");

            return errors.ToArray();
        }

        public async Task<string[]> DeleteCategoryAsync(int categoryId)
        {
            var errors = await informationsRepo.CanDeleteCategoryAsync(categoryId);
            if (errors.Any())
                return errors;

            await informationsRepo.DeleteCategoryAsync(categoryId);

            return new string[0];
        }


    }
}
