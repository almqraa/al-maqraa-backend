using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Al_Mqraa.Models
{
    public class Statistics
    {
        public int Bookmark { get; set; }
        public DateTime LastRead { get; set; }
        public int DayStreak { get; set; }
        public long TotalReadingTime {get;set;}
        [NotMapped]
        public Dictionary<DateTime, bool> DayTrackingTable { get; set; }
        [Key]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
