using InformationsApi.Models;
using System;

namespace InformationsApi.Repo
{
    public interface IInformationsRepo
    {
        CategoryModel[] GetCategories();
        CategoryModel[] GetCategories(int[] categoryIds);
        CategoryModel GetCategory(int categoryId);

        void SaveCategories(CategoryModel[] categories);

        string[] CanDeleteCategory(int categoryId);
        void DeleteCategory(int categoryId);

        InformationModel[] GetActiveInformations();
        InformationModel[] GetInformations(bool activeOnly, DateTime? fromDate);
        InformationModel GetInformation(int informationId);

        string UploadFile(string fileUrl, byte[] data);
        void SaveInformation(InformationModel information);

        void DeleteInformation(int informationId);

    }
}
