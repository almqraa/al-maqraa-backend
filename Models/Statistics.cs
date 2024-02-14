
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Statistics
    {
        public int Id { get; set; }
        public int Bookmark { get; set; }
        public DateTime LastRead { get; set; }
        public int DayStreak { get; set; }
        public long TotalReadingTime {get;set;}
        [NotMapped]
        public Dictionary<DateTime, bool> DayTrackingTable { get; set; }
        public string UserId { get; set; }
        public virtual User? User { get; set; }
    }
