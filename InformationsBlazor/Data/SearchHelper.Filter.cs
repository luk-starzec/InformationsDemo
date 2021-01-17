using InformationsBlazor.Models;
using System.Collections.Generic;
using System.Linq;

namespace InformationsBlazor.Data
{
    public static partial class SearchHelper
    {
        public static string NormalizeText(string text) => string.IsNullOrWhiteSpace(text) ? string.Empty : text.ToLower().Trim();

        public static NewestInformation[] FilterNewestInformations(NewestInformation[] newestInformations, string search)
        {
            return newestInformations == null || search.Length < 1
                ? newestInformations
                : newestInformations.Where(r => ContainsSearchedText(r, search)).ToArray();
        }

        public static Category[] FilterCategories(Category[] categories, string search)
        {
            if (categories == null || search.Length < 1)
                return categories;

            var result = new List<Category>();
            foreach (var category in categories)
            {
                if (NormalizeText(category.Title).Contains(search))
                {
                    result.Add(category);
                }
                else
                {
                    var newCategory = FilterCategoryInformations(category, search);

                    var subCategories = new List<Category>();
                    foreach (var subCategory in category.SubCategories)
                    {
                        if (NormalizeText(subCategory.Title).Contains(search))
                        {
                            subCategories.Add(subCategory);
                        }
                        else
                        {
                            var filtered = FilterCategoryInformations(subCategory, search);
                            if (filtered != null)
                                subCategories.Add(filtered);
                        }
                    }
                    if (subCategories.Any())
                    {
                        if (newCategory == null)
                            newCategory = new Category(category);

                        newCategory.SubCategories = subCategories.ToArray();
                    }
                    if (newCategory != null)
                        result.Add(newCategory);
                }
            }
            return result.ToArray();
        }


        private static Category FilterCategoryInformations(Category category, string search)
        {
            var filteredInformations = category.Informations.Where(r => ContainsSearchedText(r, search)).ToArray();

            return filteredInformations.Any() ?
                 new Category(category) { Informations = filteredInformations }
                 : null;
        }

        private static bool ContainsSearchedText(NewestInformation newestInformation, string search)
        {
            return NormalizeText(newestInformation.Title).Contains(search)
                    || NormalizeText(newestInformation.CategoryName).Contains(search)
                    || NormalizeText(newestInformation.Seasons).Contains(search);
        }

        private static bool ContainsSearchedText(Information information, string searchText)
        {
            return NormalizeText(information.Title).Contains(searchText)
                    || NormalizeText(information.Seasons).Contains(searchText);
        }
    }
}
