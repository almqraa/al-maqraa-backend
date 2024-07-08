using Al_Maqraa.DTO;
using Al_Maqraa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Composition;
using System.Net.Http.Headers;

namespace Al_Maqraa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecitationController : ControllerBase
    {
        private readonly SpeechToTextService _speechToTextService;
        private readonly QuranService _quranService;
        private static readonly HttpClient client = new HttpClient();
        private const string API_URL = "https://api-inference.huggingface.co/models/tarteel-ai/whisper-base-ar-quran";
        public static string API_TOKEN = Environment.GetEnvironmentVariable("API_TOKEN"); // Replace with your actual token
        public HashSet<char> arabicCharacters = new HashSet<char>
        {
            'ء', 'آ', 'أ', 'ؤ', 'إ', 'ئ', 'ا', 'ب', 'ة', 'ت', 'ث', 'ج', 'ح', 'خ', 'د', 'ذ', 'ر', 'ز',
            'س', 'ش', 'ص', 'ض', 'ط', 'ظ', 'ع', 'غ', 'ف', 'ق', 'ك', 'ل', 'م', 'ن', 'ه', 'و', 'ى', 'ي'
        };
        public Dictionary<char, char> tashkelMap = new Dictionary<char, char>
        {
            { 'ٱ','ا' }, //alph wasl -->alph wasl
            { 'ً', 'ً' }, // Tanween Fat'h
            { 'ٌ', 'ٌ' }, // Tanween Dham
            { 'ٞ','ٌ' }, // Tanween Dham second
            { 'ٍ', 'ٍ' }, // Tanween Kaser
            { 'َ', 'َ' }, // Fat'ha
            { 'ُ', 'ُ' }, // Dhamma
            { 'ِ', 'ِ' }, // Kasra
            { 'ّ', 'ّ' }, // Shadda
            { 'ْ', 'ْ' }, // Sekoon
            { 'ۡ', 'ْ' }, // Sekoon in quran -->sekoon
            { 'ٰ', 'ا' }, // small alph --> alph wasl
            { 'ۥ', 'ُ' }, // Small Waw --->dhamma
            { 'ۦ', 'ِ' }, // Small Yaa-->kasra 
            { 'ٔ','أ' }, //small hamza --> أ
        };
        public RecitationController(SpeechToTextService speechToTextService, QuranService quranService)
        {
            _speechToTextService = speechToTextService;
            _quranService = quranService;
        }
        [HttpGet("surah")]
        public ActionResult<List<Surah>?> GetAyahs()
        {
            return _quranService._quranData;
        }
      
        [HttpPost("recite")]
        public async Task<IActionResult> ReciteFromAudio(ReciteDTO reciteDTO)
        {
            try
            {//ff

                /*string fileName = Path.GetFileName(reciteDTO.file.FileName);
                string path = Path.Combine("wwwroot/assets/", Guid.NewGuid().ToString() + fileName);
                using (FileStream fs = System.IO.File.Create(path))
                {
                    reciteDTO.file.CopyTo(fs);
                }*/

                var memoryStream = new MemoryStream();
                await reciteDTO.file.CopyToAsync(memoryStream);
                var audioBytes = memoryStream.ToArray();
                // Decode base64 string to binary data
                //byte[] audioBytes = Convert.FromBase64String(reciteDTO.file.FileName);
                string convertedText="";
                if (reciteDTO.ModelNum == 0)
                {
                    // Send the binary data to the Huggingface API
                    var response = await SendToHuggingfaceApi(audioBytes);
                    if (response == null)
                    {
                        return StatusCode(500, "An error occurred while processing the audioo.");
                    }
                    // Extract text from the response
                    convertedText = response["text"]?.ToString();
                }
                else if(reciteDTO.ModelNum == 1)
                {
                    string base64Audio = Convert.ToBase64String(audioBytes);
                    // Convert audio data to text
                    convertedText = await _speechToTextService.ConvertToText(base64Audio);
                }
                // Check the converted text against the Quranic text
                MistakeDTO matchingWords = CheckAgainstQuranicText(convertedText,reciteDTO.SurahNum,reciteDTO.AyahNum);

                //return Ok(new { Text = convertedText, IsMatching = isMatching ,Size=isMatching.Length });
                return Ok(matchingWords);
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return StatusCode(500, "An error occurred while processing the audio.");
            }
        }

        private async Task<JObject?> SendToHuggingfaceApi(byte[] audioBytes)
        {
            using (var content = new ByteArrayContent(audioBytes))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", API_TOKEN);

                var response = await client.PostAsync(API_URL, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    return JObject.Parse(responseString);
                }
                return null;
            }
        }
        private string FilterAyah(string originalAyah) {
            string filteredAyah="";
            foreach(char ch in originalAyah)
            {
                if (arabicCharacters.Contains(ch)|| ch == ' ') filteredAyah += ch;
                else if (tashkelMap.ContainsKey(ch)) filteredAyah += tashkelMap[ch];
               
            }

            return filteredAyah;
        }
        private MistakeDTO CheckAgainstQuranicText(string recitedAyah,int surahNum,int ayahNum)
        {
            string originalAyah = _quranService.GetAyahBySurahAndNumber(surahNum, ayahNum);
            string filteredAyah = FilterAyah(originalAyah);

            string[] filteredAyahArray = filteredAyah.Split(' ');
            string[] recitedAyahArray = recitedAyah.Split(' ');
          
            for(int j=0; j < filteredAyahArray.Length; j++)
            {
                if (filteredAyahArray[j].Equals("الرَّحْمَانِ")) filteredAyahArray[j] = "الرَّحْمَنِ";
            }
            filteredAyah = String.Join(" ", filteredAyahArray);
            MistakeDTO mistakeDTO = new MistakeDTO();
            mistakeDTO.RecitedAyah = recitedAyah;
            mistakeDTO.OriginalAyah = filteredAyah;
      
           
            //because its arabic words
            filteredAyah.Reverse();
            recitedAyahArray.Reverse();
            //min value to make the compare between all the words
            //and the remains of recited will be negative
            int size = Math.Min(filteredAyahArray.Length, recitedAyahArray.Length);
            mistakeDTO.Errors = new Dictionary<string, bool>();
            bool isMistakeFound=false;
            double numOfCorrect = 0;
            int i=0;
            for (; i < size; i++)
            {
                if (recitedAyahArray[i].Equals(filteredAyahArray[i]))
                {
                    mistakeDTO.Errors.Add(recitedAyahArray[i], true);
                    numOfCorrect++;
                } 
                else
                {
                    mistakeDTO.Errors.Add(recitedAyahArray[i], false);
                    if (!isMistakeFound)
                    {
                        isMistakeFound = true;
                        mistakeDTO.IsCorrect = false;
                    }
                }
                   
            }
            //the  remaing of recited if remain 
            for (; i < recitedAyahArray.Length; i++)
            {
                    mistakeDTO.Errors.Add(recitedAyahArray[i] , false);
                    if (!isMistakeFound)
                    {
                        isMistakeFound = true;
                        mistakeDTO.IsCorrect = false;
                    }
            }
            mistakeDTO.percentage = (numOfCorrect / Convert.ToDouble(recitedAyahArray.Length)) * 100;
            return mistakeDTO;
        }
        
    }
}
