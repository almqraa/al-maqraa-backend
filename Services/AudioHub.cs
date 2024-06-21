using Microsoft.AspNetCore.SignalR;
using System.Drawing.Printing;
using System.Threading.Tasks;

public class AudioHub : Hub
{
    public async Task<string> SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
        
        return $"{message} received!";
    }
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync($"New Member ({Context.ConnectionId}): Joined");
    }

    /*public async Task SendMessage(string message)
    {
        await Clients.Caller.SendAsync($"{Context.ConnectionId}: {message}");
    }*/
    public async Task ReceiveAudio(string audioData)
    {
        // Process the audio data here
        var result = ProcessAudio(audioData);

        // Send result back to client
        await Clients.Caller.SendAsync("ReceiveResult");
    }

    private string ProcessAudio(string audioData)
    {
        // Implement your audio processing logic here
        return "Processed Audio Data";
    }

}
