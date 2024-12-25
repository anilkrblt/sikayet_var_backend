using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record ProductDto(
    int Id,
    int? BrandId,
    int? CategoryId,
    string Name,
    string? Description,
    DateTime? CreatedAt,
    DateTime? UpdatedAt
);
    public record ProductCreateDto(
        int BrandId,
        int CategoryId,
        string Name,
        string? Description
    );
}