using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class AudioHub : Hub
{
    public async Task ReceiveAudio(byte[] audioData)
    {
        // Process the audio data here
        var result = ProcessAudio(audioData);

        // Send result back to client
        await Clients.Caller.SendAsync("ReceiveResult", result);
    }

    private string ProcessAudio(byte[] audioData)
    {
        // Implement your audio processing logic here
        return "Processed Audio Data";
    }
}
