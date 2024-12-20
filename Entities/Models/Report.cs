using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int? ReporterUserId { get; set; }
        public string? TargetType { get; set; }
        public int? TargetId { get; set; }
        public string? Reason { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public User? ReporterUser { get; set; }
    }
}