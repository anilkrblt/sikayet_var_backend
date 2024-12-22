using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IServiceManager
    {
        IBrandService BrandService { get; }
        ICategoryService CategoryService { get; }
        ICommentService CommentService { get; }
        IComplaintService ComplaintService { get; }
        ILikeService LikeService { get; }
        INotificationService NotificationService { get; }
        IProductService ProductService { get; }
        IReportService ReportService { get; }
        IUserService UserService { get; }

    }
}