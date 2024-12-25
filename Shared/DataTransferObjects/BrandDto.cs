using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
  public record BrandDto(
    int Id,
    string Name,
    string? Description,
    DateTime? CreatedAt,
    DateTime? UpdatedAt
);

  public record BrandCreateDto(
      string Name,
      string? Description
  );

}