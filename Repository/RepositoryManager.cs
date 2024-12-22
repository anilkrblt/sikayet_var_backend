using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;

namespace Repository
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IBrandRepository> _brandRepository;
        private readonly Lazy<ICategoryRepository> _categoryRepository;
        private readonly Lazy<ICommentRepository> _commentRepository;
        private readonly Lazy<IComplaintRepository> _complaintRepository;
        private readonly Lazy<ILikeRepository> _likeRepository;
        private readonly Lazy<INotificationRepository> _notificationRepository;
        private readonly Lazy<IProductRepository> _productRepository;
        private readonly Lazy<IReportRepository> _reportRepository;
        private readonly Lazy<IUserRepository> _userRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _brandRepository = new Lazy<IBrandRepository>(() => new BrandRepository(repositoryContext));
            _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(repositoryContext));
            _commentRepository = new Lazy<ICommentRepository>(() => new CommentRepository(repositoryContext));
            _complaintRepository = new Lazy<IComplaintRepository>(() => new ComplaintRepository(repositoryContext));
            _likeRepository = new Lazy<ILikeRepository>(() => new LikeRepository(repositoryContext));
            _notificationRepository = new Lazy<INotificationRepository>(() => new NotificationRepository(repositoryContext));
            _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(repositoryContext));
            _reportRepository = new Lazy<IReportRepository>(() => new ReportRepository(repositoryContext));
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));

        }

        public IBrandRepository Brand => _brandRepository.Value;
        public ICategoryRepository Category => _categoryRepository.Value;
        public IComplaintRepository Complaint => _complaintRepository.Value;
        public ILikeRepository Like => _likeRepository.Value;
        public INotificationRepository Notification => _notificationRepository.Value;
        public IProductRepository Product => _productRepository.Value;
        public IReportRepository Report => _reportRepository.Value;
        public IUserRepository User => _userRepository.Value;
        public ICommentRepository Comment => _commentRepository.Value;
        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}