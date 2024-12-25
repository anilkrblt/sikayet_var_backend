using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record CategoryDto(
    int Id,
    string Name,
    string? Description,
    DateTime? CreatedAt,
    DateTime? UpdatedAt
);

    public record CategoryCreateDto(
        string Name,
        string? Description
    );
}