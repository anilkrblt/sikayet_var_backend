using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public ICollection<Complaint>? Complaints { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Report>? Reports { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}