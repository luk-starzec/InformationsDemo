using System;

namespace InformationsApi.Models
{
    public class InformationModel
    {
        public int InformationId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime Added { get; set; }
        public string FileUrl { get; set; }
        public byte[] File { get; set; }
        public int[] SeasonsId { get; set; }
    }
}
