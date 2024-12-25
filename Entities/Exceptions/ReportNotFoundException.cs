using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class ReportNotFoundException : NotFoundException
    {
        public ReportNotFoundException(int reportId)
        : base($"The report with id: {reportId} doesn't exist in the  database.")
        {
            
        }
    }
}