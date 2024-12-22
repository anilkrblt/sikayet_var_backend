using AutoMapper;
using Contracts;
using Service.Contracts;
using Service;
using Shared.DataTransferObjects;
using NLog;

namespace Service
{

    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<INotificationService> _notificationService;
        private readonly Lazy<IBrandService> _brandService;
        private readonly Lazy<ICategoryService> _categoryService;
        private readonly Lazy<ICommentService> _commentService;
        private readonly Lazy<IComplaintService> _complaintService;
        private readonly Lazy<ILikeService> _likeService;
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IReportService> _reportService;
        private readonly Lazy<IUserService> _userService;



        public ServiceManager(IRepositoryManager repositoryManager,
                            ILoggerManager logger,
                            IMapper mapper)
        {

            _userService = new Lazy<IUserService>(() =>
             new UserService(repositoryManager, logger, mapper));
             
            _reportService = new Lazy<IReportService>(() =>
           new ReportService(repositoryManager, logger, mapper));

            _productService = new Lazy<IProductService>(() =>
             new ProductService(repositoryManager, logger, mapper));
            _notificationService = new Lazy<INotificationService>(() =>
           new NotificationService(repositoryManager, logger, mapper));

            _likeService = new Lazy<ILikeService>(() =>
             new LikeService(repositoryManager, logger, mapper));
            _complaintService = new Lazy<IComplaintService>(() =>
           new ComplaintService(repositoryManager, logger, mapper));

            _commentService = new Lazy<ICommentService>(() =>
             new CommentService(repositoryManager, logger, mapper));

            _categoryService = new Lazy<ICategoryService>(() =>
           new CategoryService(repositoryManager, logger, mapper));

            _brandService = new Lazy<IBrandService>(() =>
             new BrandService(repositoryManager, logger, mapper));
        }

        public ICommentService CommentService => _commentService.Value;
        public IComplaintService ComplaintService => _complaintService.Value;
        public ILikeService LikeService => _likeService.Value;
        public INotificationService NotificationService => _notificationService.Value;
        public IProductService ProductService => _productService.Value;
        public IReportService ReportService => _reportService.Value;
        public IUserService UserService => _userService.Value;
        public IBrandService BrandService => _brandService.Value;
        public ICategoryService CategoryService => _categoryService.Value;
    }
}