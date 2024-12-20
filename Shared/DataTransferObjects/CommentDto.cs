using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
   public record CommentDto(
    int Id,
    int? ComplaintId,
    int? UserId,
    string Content,
    DateTime? CreatedAt,
    DateTime? UpdatedAt
);
}