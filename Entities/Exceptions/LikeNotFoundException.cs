using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
     public sealed class LikeNotFoundException : NotFoundException
    {
        public LikeNotFoundException(int likeId)
        : base($"The like with id: {likeId} doesn't exist in the  database.")
        {
            
        }
    }
}