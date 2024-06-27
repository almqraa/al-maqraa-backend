using System.ComponentModel.DataAnnotations;

namespace Al_Maqraa.DTO
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
