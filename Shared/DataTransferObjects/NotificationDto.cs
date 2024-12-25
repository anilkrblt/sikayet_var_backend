using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public class NotificationDto
    {
        public int UserId { get; set; }
        public string? Type { get; set; }
        public string? Content { get; set; }
        public bool? IsRead { get; set; }

        // Parameterless constructor
        public NotificationDto() { }

        // Constructor with parameters
        public NotificationDto(int userId, string? type, string? content, bool? isRead)
        {
            UserId = userId;
            Type = type;
            Content = content;
            IsRead = isRead;
        }
    }

}