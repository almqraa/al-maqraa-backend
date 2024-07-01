using System.Text.Json.Serialization;
namespace Al_Maqraa.Models
{
    public class Mistake
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateOnly Date { get; set; }
        public string SurahName { get; set; }
        public int AyahNumber { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
