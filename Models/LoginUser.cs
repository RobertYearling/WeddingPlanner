using System.ComponentModel.DataAnnotations;

namespace PlannerTwo.Models
{
    public class LoginUser
    {
        // Login Email Address
        [Required]
        [EmailAddress]
        public string LoginEmail { get; set; }

        // Login Password
        [Required]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }
    }
}