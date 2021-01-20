using InformationsApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InformationsApi.Services
{
    public interface IInformationsService
    {
        Task<InformationExtModel[]> GetInformationsAsync(bool activeOnly, DateTime? fromDate = null);
        Task<InformationModel> GetInformationAsync(int informationId);

        Task<string[]> SaveInformationAsync(InformationModel information);

        Task DeleteInformationAsync(int informationId);


        Task<CategoryModel[]> GetCategoriesAsync();
        Task<CategoryModel> GetCategoryAsync(int categoryId);

        Task<string[]> SaveCategoryAsync(CategoryModel category);
        Task<IDictionary<string, string[]>> SaveCategoriesAsync(CategoryModel[] categories);

        Task<string[]> DeleteCategoryAsync(int categoryId);
    }
}
