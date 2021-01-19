
namespace InformationsApi.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public int? ParentCategoryId { get; set; }
        public string Name { get; set; }
        public bool IsDestination { get; set; }
        public int Order { get; set; }
    }
}
