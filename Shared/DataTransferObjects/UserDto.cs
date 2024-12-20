using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
   public record UserDto(
    int Id,
    string Email,
    string PasswordHash,
    string Username,
    string Role,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
}