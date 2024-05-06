using Microsoft.AspNetCore.Identity;
public class User : IdentityUser
{
    public string Name { get; set; }
    public string? PhoneNumber { get; set; }
    public int? Gender { get; set; }
    public virtual Statistics? Statistics { get; set; }
    public virtual List<Day>? Days { get; set; }
}

