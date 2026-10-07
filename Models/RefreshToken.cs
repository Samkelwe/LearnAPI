using System.ComponentModel.DataAnnotations;

namespace LearnAPI.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        [Required]
        public string Token { get; set; } = string.Empty;

        public DateTime Expires { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public bool IsRevoked { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;

        // Link to User
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}