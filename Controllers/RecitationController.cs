using Al_Maqraa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Al_Maqraa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecitationController : ControllerBase
    {
        private readonly SpeechToTextService _speechToTextService;

        public RecitationController(SpeechToTextService speechToTextService)
        {
            _speechToTextService = speechToTextService;
        }

        [HttpPost("recite")]
        public async Task<IActionResult> ReciteFromAudio([FromBody] string audioData)
        {
            try
            {
                // Convert audio data to text
                string convertedText = await _speechToTextService.ConvertToText(audioData);

                // Check the converted text against the Quranic text
                bool isMatching = CheckAgainstQuranicText(convertedText);

                return Ok(new { Text = convertedText, IsMatching = isMatching });
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return StatusCode(500, "An error occurred while processing the audio.");
            }
        }

        private bool CheckAgainstQuranicText(string text)
        {
            // Implement logic to check text against Quranic text
            // This logic will depend on your specific requirements
            // For simplicity, let's assume it always returns true for now
            return true;
        }
    }
}
