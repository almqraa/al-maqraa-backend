
    public class Surah
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Ayah>? Ayat { get; set; }
    }

