using System;

namespace InformationsBlazor.Models
{
    public class Information
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
        public string Seasons { get; set; }


        public Information()
        { }

        public Information(Information info)
            => (Title, Url, Date, Seasons) = (info.Title, info.Url, info.Date, info.Seasons);
    }
}
