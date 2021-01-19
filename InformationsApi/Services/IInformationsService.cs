using InformationsApi.Models;
using System;
using System.Collections.Generic;

namespace InformationsApi.Services
{
    public interface IInformationsService
    {
        InformationExtModel[] GetInformations(bool activeOnly, DateTime? fromDate = null);
        InformationModel GetInformation(int informationId);

        string[] SaveInformation(InformationModel information);

        void DeleteInformation(int informationId);


        CategoryModel[] GetCategories();
        CategoryModel GetCategory(int categoryId);

        string[] SaveCategory(CategoryModel category);
        IDictionary<string, string[]> SaveCategories(CategoryModel[] categories);

        string[] DeleteCategory(int categoryId);
    }
}
