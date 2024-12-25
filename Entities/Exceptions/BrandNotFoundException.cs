using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
     public sealed class BrandNotFoundException : NotFoundException
    {
        public BrandNotFoundException()
        : base($"The brand doesn't exist in the  database.")
        {
            
        }
    }
}