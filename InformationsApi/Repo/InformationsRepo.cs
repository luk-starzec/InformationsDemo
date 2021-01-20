using InformationsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformationsApi.Repo
{
    public class InformationsRepo : IInformationsRepo
    {
        private List<CategoryModel> categoryModels;
        private List<InformationModel> informationModels;

        public InformationsRepo()
        {
            categoryModels = FakeData.GetCategories().ToList();
            informationModels = FakeData.GetInformations().ToList();
        }


        public async Task<CategoryModel[]> GetCategoriesAsync()
        {
            var result = categoryModels.ToArray();
            return await Task.FromResult(result);
        }

        public async Task<CategoryModel[]> GetCategoriesAsync(int[] categoryIds)
        {
            var result = categoryModels
                  .Where(r => categoryIds.Contains(r.CategoryId))
                  .ToArray();
            return await Task.FromResult(result);
        }

        public async Task<CategoryModel> GetCategoryAsync(int categoryId)
        {
            var result = categoryModels.SingleOrDefault(r => r.CategoryId == categoryId);
            return await Task.FromResult(result);
        }

        public async Task SaveCategoriesAsync(CategoryModel[] categories)
        {
            foreach (var category in categories)
            {
                if (category.CategoryId == 0)
                {
                    category.CategoryId = categoryModels.Select(r => r.CategoryId).Max() + 1;
                    categoryModels.Add(category);
                }
                else
                {
                    var current = categoryModels.Single(r => r.CategoryId == category.CategoryId);

                    current.ParentCategoryId = category.ParentCategoryId;
                    current.Name = category.Name;
                    current.IsDestination = category.IsDestination;
                    current.Order = category.Order;
                }
            }
            await Task.CompletedTask;
        }

        public async Task<string[]> CanDeleteCategoryAsync(int categoryId)
        {
            var errors = new List<string>();

            var categoriesCount = informationModels
                .Where(r => r.CategoryId == categoryId)
                .Count();
            if (categoriesCount > 0)
                errors.Add($"Do kategorii jest przypisanych {categoriesCount} komunikatów");

            var subcategoryIds = categoryModels
                .Where(r => r.ParentCategoryId == categoryId)
                .Select(r => r.CategoryId)
                .ToArray();
            var subcategoriesCount = informationModels
                .Where(r => subcategoryIds.Contains(r.CategoryId))
                .Count();
            if (subcategoriesCount > 0)
                errors.Add($"Do podkategorii jest przypisanych {subcategoriesCount} komunikatów");

            return await Task.FromResult(errors.ToArray());
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = categoryModels.Single(r => r.CategoryId == categoryId);
            categoryModels.Remove(category);
            await Task.CompletedTask;
        }

        public async Task<InformationModel[]> GetActiveInformationsAsync()
        {
            var time = DateTime.Now;
            var result = informationModels
                .Where(r => r.ValidFrom <= time)
                .Where(r => r.ValidTo >= time)
                .ToArray();
            return await Task.FromResult(result);
        }

        public async Task<InformationModel[]> GetInformationsAsync(bool activeOnly, DateTime? fromDate)
        {
            var query = informationModels.AsQueryable();

            if (activeOnly)
            {
                var time = DateTime.Now;
                query = query
                    .Where(r => r.ValidFrom <= time)
                    .Where(r => r.ValidTo >= time);
            }

            if (fromDate.HasValue)
            {
                query = query
                    .Where(r => r.Added >= fromDate.Value);
            }

            var result = query.ToArray();
            return await Task.FromResult(result);
        }

        public async Task<InformationModel> GetInformationAsync(int informationId)
        {
            var result = informationModels.SingleOrDefault(r => r.InformationId == informationId);
            return await Task.FromResult(result);
        }

        public async Task<string> UploadFileAsync(string fileUrl, byte[] data)
        {
            var result = FakeData.GetFileUrl();
            return await Task.FromResult(result);
        }

        public async Task SaveInformationAsync(InformationModel information)
        {
            if (information.InformationId == 0)
            {
                information.InformationId = informationModels.Select(r => r.InformationId).Max() + 1;
                information.Added = DateTime.Now;
                informationModels.Add(information);
            }
            else
            {
                var current = informationModels.Single(r => r.InformationId == information.InformationId);

                current.Title = information.Title;
                current.CategoryId = information.CategoryId;
                current.FileUrl = information.FileUrl;
                current.ValidFrom = information.ValidTo;
                current.ValidTo = information.ValidTo;
                current.SeasonsId = information.SeasonsId.ToArray();
            }
            await Task.CompletedTask;
        }

        public async Task DeleteInformationAsync(int informationId)
        {
            var information = informationModels.Single(r => r.InformationId == informationId);
            informationModels.Remove(information);
            await Task.CompletedTask;
        }
    }
}
