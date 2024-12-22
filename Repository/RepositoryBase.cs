using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    public abstract class RepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        protected IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges
                ? RepositoryContext.Set<T>().AsNoTracking()
                : RepositoryContext.Set<T>();

        protected IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges
                ? RepositoryContext.Set<T>().Where(expression).AsNoTracking()
                : RepositoryContext.Set<T>().Where(expression);

        protected void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
        protected void Update(T entity) => RepositoryContext.Set<T>().Update(entity);
        protected void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
    }
}
