using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class CommentNotFoundException : NotFoundException
    {
        public CommentNotFoundException(int commentId)
        : base($"The comment with id: {commentId} doesn't exist in the  database.")
        {
            
        }
    }
}