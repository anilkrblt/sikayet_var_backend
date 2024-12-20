using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record ReportDto(
     int Id,
     int? ReporterUserId,
     string? TargetType,
     int? TargetId,
     string? Reason,
     string? Status,
     DateTime? CreatedAt,
     DateTime? UpdatedAt
 );
}