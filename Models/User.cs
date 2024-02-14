using Microsoft.AspNetCore.Identity;

namespace Al_Mqraa.Models
{
    public class User: IdentityUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email{ get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public int StatisticsId { get; set; } 
        public Statistics Statistics { get; set; }
    }
}
