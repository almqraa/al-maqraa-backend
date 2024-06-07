

public interface ISpeechToTextRepository
{
    Task<string> ConvertToText(string audioData);
}

