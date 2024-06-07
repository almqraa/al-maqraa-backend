
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Statistics
    {
        public int Id { get; set; }
        public int? Bookmark { get; set; }
        public DateTime? LastRead { get; set; }
        public int? DayStreak { get; set; }
        public long? TotalReadingTime {get;set;}
        public string UserId { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }
    }
