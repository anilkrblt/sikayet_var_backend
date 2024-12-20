using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record NotificationDto(
     int Id,
     int? UserId,
     string? Type,
     int? ReferenceId,
     string? Content,
     bool? IsRead,
     DateTime? CreatedAt
 );
}