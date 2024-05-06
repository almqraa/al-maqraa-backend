using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

public class Day
{
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public int Score { get; set; }
        public string UserId { get; set; } 
        [JsonIgnore]
        public virtual User? User { get; set; }
}

