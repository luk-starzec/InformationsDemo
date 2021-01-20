using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace InformationsApi.Models
{
    public class InformationExtModel
    {
        public int InformationId { get; set; }
        [Required]
        public CategoryModel Category { get; set; }
        public CategoryModel Subcategory { get; set; }
        public string Title { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime Added { get; set; }
        public string FileUrl { get; set; }
        public int[] SeasonIds { get; set; }
        public string SeasonsText { get; set; }
    }
}
