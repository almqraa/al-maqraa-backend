namespace Al_Maqraa.Models
{
    public class Mistake
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int SurahNumber { get; set; }
        public int AyahNumber { get; set; }
        public string UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
