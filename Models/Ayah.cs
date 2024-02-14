namespace Al_Mqraa.Models
{
    public class Ayah
    {
        public int Id { get; set; }
        public int Verse { get; set; }
        public int SurahId { get; set;}
        public virtual Surah? Surah{ get; set;}
    }
}
