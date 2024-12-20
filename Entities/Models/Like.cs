using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ComplaintId { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Navigation Properties
        public User? User { get; set; }
        public Complaint? Complaint { get; set; }
    }
}