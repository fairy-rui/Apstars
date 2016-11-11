using Apstars.Specifications;
using Apstars.Querying;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Apstars.Repositories.EntityFramework
{
    /// <summary>
    /// Represents the Entity Framework repository.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the aggregate root.</typeparam>
    public class EntityFrameworkRepository<TKey, TEntity> : Repository<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IAggregateRoot<TKey>, new()
    {
        #region Private Fields
        private readonly IEntityFrameworkRepositoryContext efContext;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instace of <c>EntityFrameworkRepository</c> class.
        /// </summary>
        /// <param name="context">The repository context.</param>
        public EntityFrameworkRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
        #endregion

        #region Private Methods
        private MemberExpression GetMemberInfo(LambdaExpression lambda)
        {
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }

        private string GetEagerLoadingPath(Expression<Func<TEntity, dynamic>> eagerLoadingProperty)
        {
            MemberExpression memberExpression = this.GetMemberInfo(eagerLoadingProperty);
            var parameterName = eagerLoadingProperty.Parameters.First().Name;
            var memberExpressionStr = memberExpression.ToString();
            var path = memberExpressionStr.Replace(parameterName + ".", "");
            return path;
        }
        #endregion

        #region Protected Properties
        /// <summary>
        /// Gets the instance of the <see cref="IEntityFrameworkRepositoryContext"/>.
        /// </summary>
        protected IEntityFrameworkRepositoryContext EFContext
        {
            get { return efContext; }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Adds an aggregate root to the repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be added to the repository.</param>
        protected override void DoAdd(TEntity aggregateRoot)
        {
            efContext.RegisterNew(aggregateRoot);
        }
        /// <summary>
        /// Gets the aggregate root instance from repository by a given key.
        /// </summary>
        /// <param name="key">The key of the aggregate root.</param>
        /// <returns>The instance of the aggregate root.</returns>
        protected override TEntity DoGetByKey(TKey key)
        {
            return
                efContext.Context.Set<TEntity>()
                    .Where(Utils.BuildIdEqualsPredicate<TKey, TEntity>((TKey)key))
                    .First();
        }

        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>The aggregate roots.</returns>
        protected override IQueryable<TEntity> DoFindAll(
            ISpecification<TEntity> specification,
            Expression<Func<TEntity, dynamic>> sortPredicate,
            SortOrder sortOrder)
        {
            var query = efContext.Context.Set<TEntity>().Where(specification.Expression);
            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.Ascending:
                        return query.SortBy<TKey, TEntity>(sortPredicate);
                    case SortOrder.Descending:
                        return query.SortByDescending<TKey, TEntity>(sortPredicate);
                    default:
                        break;
                }
            }
            return query;
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
        protected override PagedResult<TKey, TEntity> DoFindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "The pageNumber is one-based and should be larger than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "The pageSize is one-based and should be larger than zero.");
            if (sortPredicate == null)
                throw new ArgumentNullException("sortPredicate");
           
            var query = efContext.Context.Set<TEntity>()
                .Where(specification.Expression);
            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;

            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    var pagedGroupAscending = query.SortBy<TKey, TEntity>(sortPredicate).Skip(skip).Take(take).GroupBy(p => new { Total = query.Count() }).FirstOrDefault();
                    if (pagedGroupAscending == null)
                        return null;
                    return new PagedResult<TKey, TEntity>(pagedGroupAscending.Key.Total, (pagedGroupAscending.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, pagedGroupAscending.Select(p => p).ToList());
                case SortOrder.Descending:
                    var pagedGroupDescending = query.SortByDescending<TKey, TEntity>(sortPredicate).Skip(skip).Take(take).GroupBy(p => new { Total = query.Count() }).FirstOrDefault();
                    if (pagedGroupDescending == null)
                        return null;
                    return new PagedResult<TKey, TEntity>(pagedGroupDescending.Key.Total, (pagedGroupDescending.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, pagedGroupDescending.Select(p => p).ToList());
                default:
                    break;
            }

            return null;
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Querying.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        protected override IQueryable<TEntity> DoFindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            var dbset = efContext.Context.Set<TEntity>();
            IQueryable<TEntity> queryable = null;
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                queryable = dbquery.Where(specification.Expression);
            }
            else
                queryable = dbset.Where(specification.Expression);

            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.Ascending:
                        return queryable.SortBy<TKey, TEntity>(sortPredicate);
                    case SortOrder.Descending:
                        return queryable.SortByDescending<TKey, TEntity>(sortPredicate);
                    default:
                        break;
                }
            }
            return queryable;
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
        protected override PagedResult<TKey, TEntity> DoFindAll(ISpecification<TEntity> specification, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "The pageNumber is one-based and should be larger than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "The pageSize is one-based and should be larger than zero.");
            if (sortPredicate == null)
                throw new ArgumentNullException("sortPredicate");

            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;

            var dbset = efContext.Context.Set<TEntity>();
            IQueryable<TEntity> queryable = null;
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                queryable = dbquery.Where(specification.Expression);
            }
            else
                queryable = dbset.Where(specification.Expression);


            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    var pagedGroupAscending = queryable.SortBy<TKey, TEntity>(sortPredicate).Skip(skip).Take(take).GroupBy(p => new { Total = queryable.Count() }).FirstOrDefault();
                    if (pagedGroupAscending == null)
                        return null;
                    return new PagedResult<TKey, TEntity>(pagedGroupAscending.Key.Total, (pagedGroupAscending.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, pagedGroupAscending.Select(p => p).ToList());
                case SortOrder.Descending:
                    var pagedGroupDescending = queryable.SortByDescending<TKey, TEntity>(sortPredicate).Skip(skip).Take(take).GroupBy(p => new { Total = queryable.Count() }).FirstOrDefault();
                    if (pagedGroupDescending == null)
                        return null;
                    return new PagedResult<TKey, TEntity>(pagedGroupDescending.Key.Total, (pagedGroupDescending.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, pagedGroupDescending.Select(p => p).ToList());
                default:
                    break;
            }

            return null;
        }

        /// <summary>
        /// Finds a single aggregate root from the repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>The instance of the aggregate root.</returns>
        protected override TEntity DoFind(ISpecification<TEntity> specification)
        {
            return efContext.Context.Set<TEntity>().Where(specification.IsSatisfiedBy).FirstOrDefault();
        }
        /// <summary>
        /// Finds a single aggregate root from the repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        protected override TEntity DoFind(ISpecification<TEntity> specification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            var dbset = efContext.Context.Set<TEntity>();
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                return dbquery.Where(specification.Expression).FirstOrDefault();
            }
            else
                return dbset.Where(specification.Expression).FirstOrDefault();
        }
        /// <summary>
        /// Checkes whether the aggregate root, which matches the given specification, exists in the repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>True if the aggregate root exists, otherwise false.</returns>
        protected override bool DoExists(ISpecification<TEntity> specification)
        {
            var count = efContext.Context.Set<TEntity>().Count(specification.IsSatisfiedBy);
            return count != 0;
        }
        /// <summary>
        /// Removes the aggregate root from current repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be removed.</param>
        protected override void DoRemove(TEntity aggregateRoot)
        {
            efContext.RegisterDeleted(aggregateRoot);
        }
        /// <summary>
        /// Updates the aggregate root in the current repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be updated.</param>
        protected override void DoUpdate(TEntity aggregateRoot)
        {
            efContext.RegisterModified(aggregateRoot);
        }

        #region New Methods  
            
        protected override async Task<TEntity> DoGetByKeyAsync(TKey key)
        {
            return await efContext.Context.Set<TEntity>().FindAsync(key);
        }
        protected override IQueryable<TEntity> DoFindAll(ISpecification<TEntity> specification, SortSpecification<TKey, TEntity> sortSpecification)
        {
            var query = efContext.Context.Set<TEntity>().Where(specification.Expression);
            IOrderedQueryable<TEntity> orderedQueryable;
            if (sortSpecification?.Count > 0)
            {
                var sortSpecificationList = sortSpecification.Specifications.ToList();
                var firstSortSpecification = sortSpecificationList[0];
                switch (firstSortSpecification.Item2)
                {
                    case SortOrder.Ascending:
                        orderedQueryable = query.SortBy<TKey, TEntity>(firstSortSpecification.Item1);//query.OrderBy(firstSortSpecification.Item1);
                        break;
                    case SortOrder.Descending:
                        orderedQueryable = query.SortByDescending<TKey, TEntity>(firstSortSpecification.Item1);
                        break;
                    default:
                        return query;
                }
                for (var i = 1; i < sortSpecificationList.Count; i++)
                {
                    var spec = sortSpecificationList[i];
                    switch (spec.Item2)
                    {
                        case SortOrder.Ascending:
                            orderedQueryable = orderedQueryable.SortThenBy<TKey, TEntity>(spec.Item1);
                            break;
                        case SortOrder.Descending:
                            orderedQueryable = orderedQueryable.SortThenByDescending<TKey, TEntity>(spec.Item1);
                            break;
                        default:
                            continue;
                    }
                }
                return orderedQueryable;
            }
            return query;
        }
        protected override PagedResult<TKey, TEntity> DoFindAll(ISpecification<TEntity> specification, SortSpecification<TKey, TEntity> sortSpecification, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "The pageNumber is one-based and should be larger than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "The pageSize is one-based and should be larger than zero.");

            var query = DoFindAll(specification, sortSpecification);
            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;

            var pagedGroup = query.Skip(skip).Take(take).GroupBy(p => new { Total = query.Count() }).FirstOrDefault();
            if (pagedGroup == null)
                return null;
            return new PagedResult<TKey, TEntity>(pagedGroup.Key.Total, (pagedGroup.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, pagedGroup.Select(p => p).ToList());
        }
        protected override IQueryable<TEntity> DoFindAll(ISpecification<TEntity> specification, SortSpecification<TKey, TEntity> sortSpecification, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            var dbset = efContext.Context.Set<TEntity>();
            IQueryable<TEntity> queryable = null;
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                var dbquery = dbset.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    dbquery = dbquery.Include(eagerLoadingPath);
                }
                queryable = dbquery.Where(specification.Expression);
            }
            else
                queryable = dbset.Where(specification.Expression);

            IOrderedQueryable<TEntity> orderedQueryable;
            if (sortSpecification?.Count > 0)
            {
                var sortSpecificationList = sortSpecification.Specifications.ToList();
                var firstSortSpecification = sortSpecificationList[0];
                switch (firstSortSpecification.Item2)
                {
                    case SortOrder.Ascending:
                        orderedQueryable = queryable.SortBy<TKey, TEntity>(firstSortSpecification.Item1);
                        break;
                    case SortOrder.Descending:
                        orderedQueryable = queryable.SortByDescending<TKey, TEntity>(firstSortSpecification.Item1);
                        break;
                    default:
                        return queryable;
                }
                for (var i = 1; i < sortSpecificationList.Count; i++)
                {
                    var spec = sortSpecificationList[i];
                    switch (spec.Item2)
                    {
                        case SortOrder.Ascending:
                            orderedQueryable = orderedQueryable.SortThenBy<TKey, TEntity>(spec.Item1);
                            break;
                        case SortOrder.Descending:
                            orderedQueryable = orderedQueryable.SortThenByDescending<TKey, TEntity>(spec.Item1);
                            break;
                        default:
                            continue;
                    }
                }
                return orderedQueryable;
            }
            return queryable;
        }
        protected override PagedResult<TKey, TEntity> DoFindAll(ISpecification<TEntity> specification, SortSpecification<TKey, TEntity> sortSpecification, int pageNumber, int pageSize, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "The pageNumber is one-based and should be larger than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "The pageSize is one-based and should be larger than zero.");

            var query = DoFindAll(specification, sortSpecification, eagerLoadingProperties);

            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;

            var pagedGroup = query.Skip(skip).Take(take).GroupBy(p => new { Total = query.Count() }).FirstOrDefault();
            if (pagedGroup == null)
                return null;
            return new PagedResult<TKey, TEntity>(pagedGroup.Key.Total, (pagedGroup.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, pagedGroup.Select(p => p).ToList());
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// Represents the Entity Framework repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the aggregate root.</typeparam>
    public class EntityFrameworkRepository<TEntity> : EntityFrameworkRepository<Guid, TEntity>,
                                                             IRepository<TEntity>
        where TEntity : class, IAggregateRoot, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The repository context.</param>
        public EntityFrameworkRepository(IRepositoryContext context)
            : base(context)
        {
        }
    }
}
