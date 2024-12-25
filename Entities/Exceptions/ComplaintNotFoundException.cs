using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class ComplaintNotFoundException : NotFoundException
    {
        public ComplaintNotFoundException(int complaintId)
        : base($"The complaint with id: {complaintId} doesn't exist in the  database.")
        {
            
        }
    }
}