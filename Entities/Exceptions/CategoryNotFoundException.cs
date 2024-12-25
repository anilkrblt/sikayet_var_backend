using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(int categoryId)
        : base($"The category with id: {categoryId} doesn't exist in the  database.")
        {
            
        }
    }
}