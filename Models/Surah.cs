namespace Al_Mqraa.Models
{
    public class Surah
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public virtual List<Ayah>? Ayat { get; set; }
    }
}
