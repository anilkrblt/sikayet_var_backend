using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record ComplaintDto(
     int Id,
     int? UserId,
     int? ProductId,
     string Title,
     string Description,
     string? Status,
     DateTime? CreatedAt,
     DateTime? UpdatedAt
 );

    public record ComplaintCreateDto(
        int UserId,
        int ProductId,
        string Title,
        string Description);
}