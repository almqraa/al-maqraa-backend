using Microsoft.AspNetCore.Identity;
public class User : IdentityUser
{
    public string? PhoneNumber { get; set; }
    public int? Gender { get; set; }
    public int? StatisticsId { get; set; }
    public Statistics? Statistics { get; set; }
}

