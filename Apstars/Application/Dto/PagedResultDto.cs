using System;
using System.Collections.Generic;

namespace Apstars.Application.Dto
{
    /// <summary>
    /// Implements <see cref="IPagedResult{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">Type of the items in the <see cref="ListResultDto{TEntity}.Items"/> list</typeparam>
    [Serializable]
    public class PagedResultDto<TKey, TEntity> : ListResultDto<TKey, TEntity>, IPagedResult<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntityDto<TKey>, new()
    {
        /// <summary>
        /// Total count of Items.
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// Gets or sets the total pages available.
        /// </summary>
        public int TotalPages { get; set; }       
        /// <summary>
        /// Gets or sets the number of records for each page.
        /// </summary>
        public int PageSize { get; set; }       
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int PageNumber { get; set; }        
        /// <summary>
        /// Creates a new <see cref="PagedResultDto{TEntity}"/> object.
        /// </summary>
        public PagedResultDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="PagedResultDto{TEntity}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="totalPages">Total number of pages.</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="items">List of items in current page</param>
        public PagedResultDto(int totalCount, int totalPages, int pageSize, int pageNumber, IReadOnlyList<TEntity> items)
            : base(items)
        {
            TotalCount = totalCount;
            TotalPages = totalPages;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }

    /// <summary>
    /// Implements <see cref="IPagedResult{TEntity}"/>.
    /// </summary>    
    /// <typeparam name="TEntity">Type of the items in the <see cref="ListResultDto{TEntity}.Items"/> list</typeparam>
    [Serializable]
    public class PagedResultDto<TEntity> : PagedResultDto<Guid, TEntity>        
        where TEntity : class, IEntityDto<Guid>, new()
    {
        /// <summary>
        /// Creates a new <see cref="PagedResultDto{TEntity}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="totalPages">Total number of pages.</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="items">List of items in current page</param>
        public PagedResultDto(int totalCount, int totalPages, int pageSize, int pageNumber, IReadOnlyList<TEntity> items)
            : base(totalCount, totalPages, pageSize, pageNumber, items)
        {
            
        }
    }
}
