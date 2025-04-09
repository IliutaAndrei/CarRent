using System.ComponentModel.DataAnnotations;

namespace CarRent.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public required string  FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
    }
}
