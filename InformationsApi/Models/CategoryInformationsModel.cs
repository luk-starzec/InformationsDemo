
namespace InformationsApi.Models
{
    public class CategoryInformationsModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public InformationModel[] Informations { get; set; }
        public CategoryInformationsModel[] SubCategories { get; set; }
    }
}
