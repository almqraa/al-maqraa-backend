using System.ComponentModel.DataAnnotations.Schema;

namespace Al_Maqraa.DTO
{
    public class StatisticsDTO
    {
        public int? Bookmark { get; set; }
        public DateTime? LastRead { get; set; }
        public int? DayStreak { get; set; }
        public long? TotalReadingTime { get; set; }

    }
}
