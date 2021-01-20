
namespace InformationsBlazor.Models
{
    public class Category
    {
        public string Title { get; set; }
        public CategoryType Type { get; set; }
        public Information[] Informations { get; set; } = new Information[0];
        public Category[] SubCategories { get; set; } = new Category[0];
        public int Order { get; set; }


        public Category()
        { }

        public Category(Category category)
            => (Title, Type, Order) = (category.Title, category.Type, category.Order);
    }
}
