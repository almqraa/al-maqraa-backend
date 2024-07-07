namespace Al_Maqraa.DTO
{
    public class ReciteDTO
    {//ff
        public int ModelNum { get; set; } = 0;
        public int SurahNum { get; set; }
        public int AyahNum { get; set; }
        public IFormFile file { get; set; }
    }
}
