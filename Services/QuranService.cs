using Newtonsoft.Json;
using System.Net;

namespace Al_Maqraa.Services
{
    public class QuranService
    {
        public List<Surah>? _quranData { get; set; }    
        public QuranService()
        {
            var jsonData = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Data/Quran.json"));
            _quranData = JsonConvert.DeserializeObject<List<Surah>>(jsonData);
        }

        public Surah? GetSurahByNumber(int number)
        {
            return _quranData?.FirstOrDefault(s => s.id == number);
        }

        public string? GetAyahBySurahAndNumber(int surahNumber, int ayahNumber)
        {
            var surah = GetSurahByNumber(surahNumber);
            return surah?.array.FirstOrDefault(a => a.id== ayahNumber).ar;
        }
    }
}
