
    public class Ayah
    {
        public int Id { get; set; }
        public string Verse { get; set; }
        public int SurahId { get; set;}
        public virtual Surah? Surah{ get; set;}
    }

