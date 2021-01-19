using System;

namespace SeasonsApi.Models
{
    public class SeasonModel
    {
        public int SeasonId { get; set; }

        public string Name { get; set; }
        //public string Url { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }

        //public DateTime ActiveFrom { get; set; }
        //public DateTime ActiveTo { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
