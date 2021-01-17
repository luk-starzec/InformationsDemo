using InformationsBlazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InformationsBlazor.Data
{
    public static partial class SearchHelper
    {
        public static NewestInformation[] HighlightSearch(NewestInformation[] newestInformations, string search)
        {
            if (newestInformations == null || string.IsNullOrWhiteSpace(search))
                return newestInformations;

            return newestInformations
                .Select(r => new NewestInformation(r)
                {
                    Title = r.Title.HighlightText(search),
                    CategoryName = r.CategoryName.HighlightText(search),
                    Seasons = r.Seasons.HighlightText(search),
                }
                ).ToArray();
        }

        public static Category[] HighlightSearch(Category[] categories, string search)
        {
            if (categories == null || string.IsNullOrWhiteSpace(search))
                return categories;

            return categories
                .Select(r => new Category(r)
                {
                    Title = r.Title.HighlightText(search),
                    Informations = HighlightSearch(r.Informations, search),
                    SubCategories = HighlightSearch(r.SubCategories, search),
                }).ToArray();

        }


        private static Information[] HighlightSearch(Information[] informations, string search)
        {
            if (informations == null || string.IsNullOrWhiteSpace(search))
                return informations;

            return informations
                .Select(r => new Information(r)
                {
                    Title = r.Title.HighlightText(search),
                    Seasons = r.Seasons.HighlightText(search),
                }
                ).ToArray();
        }

        private static string HighlightText(this string text, string search, string cssClass = "highlighted")
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(search))
                return text;

            return Regex.Replace(text, search, $"<span class=\"{cssClass}\">$0</span>", RegexOptions.IgnoreCase);
        }
    }
}
