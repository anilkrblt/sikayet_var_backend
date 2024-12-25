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
     string Role
 );


    public class UserForRegistrationDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserForUpdateDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }

}