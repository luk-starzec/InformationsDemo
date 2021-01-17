using InformationsBlazor.Models;
using System.Linq;

namespace InformationsBlazor.Data
{
    public static partial class SearchHelper
    {
        public static NewestInformation[] SortNewestInformations(NewestInformation[] newestInformations, SortingType sorting)
        {
            return SortInformations(newestInformations, sorting).Cast<NewestInformation>().ToArray();
        }


        public static Category[] SortCategoriesInformations(Category[] categories, SortingType sorting)
        {
            if (categories == null)
                return null;

            return categories.Select(r =>
                new Category(r)
                {
                    Informations = SortInformations(r.Informations, sorting),
                    SubCategories = SortCategoriesInformations(r.SubCategories, sorting),
                }).ToArray();
        }


        private static Information[] SortInformations(Information[] informations, SortingType sorting)
        {
            if (informations == null)
                return null;

            switch (sorting)
            {
                case SortingType.FromNewest:
                    return informations.OrderByDescending(r => r.Date).ThenBy(r => r.Title).ToArray();
                case SortingType.Alphabetical:
                    return informations.OrderBy(r => r.Title).ToArray();
                default:
                    return informations;
            }
        }
    }
}
