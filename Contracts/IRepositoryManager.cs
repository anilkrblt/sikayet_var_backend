using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryManager
    {
        IBrandRepository Brand { get; }
        ICategoryRepository Category { get; }
        ICommentRepository Comment { get; }
        IComplaintRepository Complaint { get; }
        ILikeRepository Like { get; }
        INotificationRepository Notification { get; }
        IProductRepository Product { get; }
        IReportRepository Report { get; }
        IUserRepository User { get; }

        Task SaveAsync();

    }
}