namespace Al_Maqraa.DTO
{
    public class MistakeDTO
    {
        public string RecitedAyah { get; set; }
        public string OriginalAyah { get; set; }
        public Dictionary<string,bool> Errors { get; set;} = new Dictionary<string, bool>();
        public bool IsCorrect { get; set; } = true;
        public double percentage { get; set; }
    }
}
