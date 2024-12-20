using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Type { get; set; }
        public int? ReferenceId { get; set; }
        public string? Content { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Navigation Properties
        public User? User { get; set; }
    }
}