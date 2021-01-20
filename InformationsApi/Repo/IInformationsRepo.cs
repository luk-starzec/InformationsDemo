using InformationsApi.Models;
using System;
using System.Threading.Tasks;

namespace InformationsApi.Repo
{
    public interface IInformationsRepo
    {
        Task<CategoryModel[]> GetCategoriesAsync();
        Task<CategoryModel[]> GetCategoriesAsync(int[] categoryIds);
        Task<CategoryModel> GetCategoryAsync(int categoryId);

        Task SaveCategoriesAsync(CategoryModel[] categories);

        Task<string[]> CanDeleteCategoryAsync(int categoryId);
        Task DeleteCategoryAsync(int categoryId);


        Task<InformationModel[]> GetActiveInformationsAsync();
        Task<InformationModel[]> GetInformationsAsync(bool activeOnly, DateTime? fromDate);
        Task<InformationModel> GetInformationAsync(int informationId);

        Task<string> UploadFileAsync(string fileUrl, byte[] data);
        Task SaveInformationAsync(InformationModel information);

        Task DeleteInformationAsync(int informationId);

    }
}
