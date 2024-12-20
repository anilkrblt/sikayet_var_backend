using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int? ComplaintId { get; set; }
        public int? UserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public Complaint? Complaint { get; set; }
        public User? User { get; set; }
    }
}