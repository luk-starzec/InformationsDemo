
namespace InformationsBlazor.Models
{
    public class NewestInformation : Information
    {
        public string CategoryName { get; set; }

        public NewestInformation()
        { }

        public NewestInformation(NewestInformation info)
            : base(info)
            => (CategoryName) = (info.CategoryName);


    }
}
