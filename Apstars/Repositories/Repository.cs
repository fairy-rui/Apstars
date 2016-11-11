using Apstars.Querying;
using Apstars.Specifications;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Apstars.Repositories
{
    /// <summary>
    /// Represents the base class for repositories.
    /// </summary>
    /// <typeparam name="TKey">The type of the key of the aggregate root.</typeparam>
    /// <typeparam name="TEntity">The type of the aggregate root on which the repository operations
    /// should be performed.</typeparam>
    public abstract class Repository<TKey, TEntity> : IRepository<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IAggregateRoot<TKey>, new()
    {
        #region Private Fields
        private readonly IRepositoryContext context;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>Repository&lt;TEntity&gt;</c> class.
        /// </summary>
        /// <param name="context">The repository context being used by the repository.</param>
        public Repository(IRepositoryContext context)
        {
            this.context = context;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Adds an aggregate root to the repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be added to the repository.</param>
        protected abstract void DoAdd(TEntity aggregateRoot);
        /// <summary>
        /// Gets the aggregate root instance from repository by a given key.
        /// </summary>
        /// <param name="key">The key of the aggregate root.</param>
        /// <returns>The instance of the aggregate root.</returns>
        protected abstract TEntity DoGetByKey(TKey key);
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <returns>All the aggregate roots got from the repository.</returns>
        protected virtual IQueryable<TEntity> DoFindAll()
        {
            return DoFindAll(new AnySpecification<TEntity>(), null, Querying.SortOrder.Unspecified);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository, sorting by using the provided sort predicate
        /// and the specified sort order.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>All the aggregate roots got from the repository, with the aggregate roots being sorted by
        /// using the provided sort predicate and the sort order.</returns>
        protected virtual IQueryable<TEntity> DoFindAll(Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder)
        {
            return DoFindAll(new AnySpecification<TEntity>(), sortPredicate, sortOrder);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository with paging enabled, sorting by using the provided sort predicate
        /// and the specified sort order.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>All the aggregate roots got from the repository, with the aggregate roots being sorted by
        /// using the provided sort predicate and the sort order.</returns>
        protected virtual PagedResult<TKey, TEntity> DoFindAll(Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder, int pageNumber, int pageSize)
        {
            return DoFindAll(new AnySpecification<TEntity>(), sortPredicate, sortOrder, pageNumber, pageSize);
        }
        /// <summary>
        /// Finds all the aggregate roots that match the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <returns>All the aggregate roots that match the given specification.</returns>
        protected virtual IQueryable<TEntity> DoFindAll(ISpecification<TEntity> specification)
        {
            return DoFindAll(specification, null, Querying.SortOrder.Unspecified);
        }
        /// <summary>
        /// Finds all the aggregate roots that match the given specification, and sorts the aggregate roots
        /// by using the provided sort predicate and the specified sort order.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        protected abstract IQueryable<TEntity> DoFindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder);
        /// <summary>
        /// Finds all the aggregate roots that match the given specification with paging enabled, and sorts the aggregate roots
        /// by using the provided sort predicate and the specified sort order.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The number of objects per page.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        protected abstract PagedResult<TKey, TEntity> DoFindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder, int pageNumber, int pageSize);
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        protected virtual IQueryable<TEntity> DoFindAll(params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return DoFindAll(new AnySpecification<TEntity>(), null, Querying.SortOrder.Unspecified, eagerLoadingProperties);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        protected virtual IQueryable<TEntity> DoFindAll(Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return DoFindAll(new AnySpecification<TEntity>(), sortPredicate, sortOrder, eagerLoadingProperties);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        protected virtual PagedResult<TKey, TEntity> DoFindAll(Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return DoFindAll(new AnySpecification<TEntity>(), sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        protected virtual IQueryable<TEntity> DoFindAll(ISpecification<TEntity> specification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return DoFindAll(new AnySpecification<TEntity>(), null, Querying.SortOrder.Unspecified, eagerLoadingProperties);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        protected abstract IQueryable<TEntity> DoFindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        protected abstract PagedResult<TKey, TEntity> DoFindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// Finds a single aggregate root that matches the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>The instance of the aggregate root.</returns>
        protected abstract TEntity DoFind(ISpecification<TEntity> specification);
        /// <summary>
        /// Finds a single aggregate root from the repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        protected abstract TEntity DoFind(ISpecification<TEntity> specification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// Checkes whether the aggregate root, which matches the given specification, exists in the repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>True if the aggregate root exists, otherwise false.</returns>
        protected abstract bool DoExists(ISpecification<TEntity> specification);
        /// <summary>
        /// Removes the aggregate root from current repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be removed.</param>
        protected abstract void DoRemove(TEntity aggregateRoot);
        /// <summary>
        /// Updates the aggregate root in the current repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be updated.</param>
        protected abstract void DoUpdate(TEntity aggregateRoot);

        #region New Methods
        protected abstract Task<TEntity> DoGetByKeyAsync(TKey key);
        protected virtual IQueryable<TEntity> DoFindAll(SortSpecification<TKey, TEntity> sortSpecification)
        {
            return this.DoFindAll(new AnySpecification<TEntity>(), sortSpecification);
        }
        protected abstract IQueryable<TEntity> DoFindAll(ISpecification<TEntity> specification, SortSpecification<TKey, TEntity> sortSpecification);
        protected virtual PagedResult<TKey, TEntity> DoFindAll(SortSpecification<TKey, TEntity> sortSpecification, int pageNumber, int pageSize)
        {
            return DoFindAll(new AnySpecification<TEntity>(), sortSpecification, pageNumber, pageSize);
        }
        protected abstract PagedResult<TKey, TEntity> DoFindAll(ISpecification<TEntity> specification, SortSpecification<TKey, TEntity> sortSpecification, int pageNumber, int pageSize);

        protected virtual IQueryable<TEntity> DoFindAll(SortSpecification<TKey, TEntity> sortSpecification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return DoFindAll(new AnySpecification<TEntity>(), sortSpecification, eagerLoadingProperties);
        }
        protected virtual PagedResult<TKey, TEntity> DoFindAll(SortSpecification<TKey, TEntity> sortSpecification, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return DoFindAll(new AnySpecification<TEntity>(), sortSpecification, pageNumber, pageSize, eagerLoadingProperties);
        }
        protected abstract IQueryable<TEntity> DoFindAll(ISpecification<TEntity> specification, SortSpecification<TKey, TEntity> sortSpecification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        protected abstract PagedResult<TKey, TEntity> DoFindAll(ISpecification<TEntity> specification, SortSpecification<TKey, TEntity> sortSpecification, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);

        #endregion

        #endregion

        #region IRepository<TEntity> Members
        /// <summary>
        /// Gets the <see cref="Repositories.IRepositoryContext"/> instance.
        /// </summary>
        public IRepositoryContext Context
        {
            get { return this.context; }
        }
        /// <summary>
        /// Adds an aggregate root to the repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be added to the repository.</param>
        public void Add(TEntity aggregateRoot)
        {
            this.DoAdd(aggregateRoot);
        }
        /// <summary>
        /// Gets the aggregate root instance from repository by a given key.
        /// </summary>
        /// <param name="key">The key of the aggregate root.</param>
        /// <returns>The instance of the aggregate root.</returns>
        public TEntity GetByKey(TKey key)
        {
            return this.DoGetByKey(key);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <returns>The aggregate roots.</returns>
        public IQueryable<TEntity> FindAll()
        {
            return this.DoFindAll();
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>The aggregate roots.</returns>
        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder)
        {
            return this.DoFindAll(sortPredicate, sortOrder);
        }
        /// <summary>
        /// Finds all the aggregate roots that match the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <returns>The aggregate roots.</returns>
        public IQueryable<TEntity> FindAll(ISpecification<TEntity> specification)
        {
            return this.DoFindAll(specification);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>The aggregate roots.</returns>
        public IQueryable<TEntity> FindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder)
        {
            return this.DoFindAll(specification, sortPredicate, sortOrder);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>The aggregate roots.</returns>
        public PagedResult<TKey, TEntity> FindAll(Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder, int pageNumber, int pageSize)
        {
            return this.DoFindAll(sortPredicate, sortOrder, pageNumber, pageSize);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The number of objects per page.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>The aggregate roots.</returns>
        public PagedResult<TKey, TEntity> FindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder, int pageNumber, int pageSize)
        {
            return this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        public IQueryable<TEntity> FindAll(params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(eagerLoadingProperties);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(sortPredicate, sortOrder, eagerLoadingProperties);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        public PagedResult<TKey, TEntity> FindAll(Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        public IQueryable<TEntity> FindAll(ISpecification<TEntity> specification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(specification, eagerLoadingProperties);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        public IQueryable<TEntity> FindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(specification, sortPredicate, sortOrder, eagerLoadingProperties);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        public PagedResult<TKey, TEntity> FindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, Querying.SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }
        /// <summary>
        /// Finds a single aggregate root that matches the given specification.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>The instance of the aggregate root.</returns>
        public TEntity Find(ISpecification<TEntity> specification)
        {
            return this.DoFind(specification);
        }
        /// <summary>
        /// Finds a single aggregate root from the repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        public TEntity Find(ISpecification<TEntity> specification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFind(specification, eagerLoadingProperties);
        }
        /// <summary>
        /// Removes the aggregate root from current repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be removed.</param>
        public void Remove(TEntity aggregateRoot)
        {
            this.DoRemove(aggregateRoot);
        }
        /// <summary>
        /// Updates the aggregate root in the current repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be updated.</param>
        public void Update(TEntity aggregateRoot)
        {
            this.DoUpdate(aggregateRoot);
        }
        /// <summary>
        /// Checkes whether the aggregate root, which matches the given specification, exists in the repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>True if the aggregate root exists, otherwise false.</returns>
        public bool Exists(ISpecification<TEntity> specification)
        {
            return this.DoExists(specification);
        }

        #region New Methods
        public Task<TEntity> GetByKeyAsync(TKey key)
        {
            return this.DoGetByKeyAsync(key);
        }
        public IQueryable<TEntity> FindAll(SortSpecification<TKey, TEntity> sortSpecification)
        {
            return this.DoFindAll(sortSpecification);
        }
        public PagedResult<TKey, TEntity> FindAll(SortSpecification<TKey, TEntity> sortSpecification, int pageNumber, int pageSize)
        {
            return this.DoFindAll(sortSpecification, pageNumber, pageSize);
        }
        public IQueryable<TEntity> FindAll(ISpecification<TEntity> specification, SortSpecification<TKey, TEntity> sortSpecification)
        {
            return this.DoFindAll(specification, sortSpecification);
        }
        public PagedResult<TKey, TEntity> FindAll(ISpecification<TEntity> specification, SortSpecification<TKey, TEntity> sortSpecification, int pageNumber, int pageSize)
        {
            return this.DoFindAll(specification, sortSpecification, pageNumber, pageSize);
        }

        public IQueryable<TEntity> FindAll(SortSpecification<TKey, TEntity> sortSpecification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(sortSpecification, eagerLoadingProperties);
        }
        public PagedResult<TKey, TEntity> FindAll(SortSpecification<TKey, TEntity> sortSpecification, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(sortSpecification, pageNumber, pageSize, eagerLoadingProperties);
        }
        public IQueryable<TEntity> FindAll(ISpecification<TEntity> specification, SortSpecification<TKey, TEntity> sortSpecification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(specification, sortSpecification, eagerLoadingProperties);
        }
        public PagedResult<TKey, TEntity> FindAll(ISpecification<TEntity> specification, SortSpecification<TKey, TEntity> sortSpecification, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(specification, sortSpecification, pageNumber, pageSize, eagerLoadingProperties);
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// Represents that the implemented classes are repositories.
    /// </summary>
    /// <typeparam name="TEntity">The type of the aggregate root.</typeparam>
    public abstract class Repository<TEntity> : Repository<Guid, TEntity>, IRepository<TEntity>
        where TEntity : class, IAggregateRoot, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The repository context being used by the repository.</param>
        public Repository(IRepositoryContext context)
            : base(context)
        {

        }
    }
}
