using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record LikeDto(
     int Id,
     int? UserId,
     int? ComplaintId,
     DateTime? CreatedAt
 );
}