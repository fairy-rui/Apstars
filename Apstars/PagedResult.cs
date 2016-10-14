using System;
using System.Collections.Generic;

namespace Apstars
{
    /// <summary>
    /// Represents a collection which contains a set of objects that is from
    /// a specific page of the entire object set.
    /// </summary>
    /// <typeparam name="TKey">The type of the key of aggregateRoot.</typeparam>
    /// <typeparam name="TAggregateRoot">The type of the aggregateRoot.</typeparam>
    public class PagedResult<TKey, TAggregateRoot> : ICollection<TAggregateRoot>
        where TKey : IEquatable<TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>, new()
    {
        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>PagedResult</c> class.
        /// </summary>
        public PagedResult()
        {
            this.entities = new List<TAggregateRoot>();
        }
        /// <summary>
        /// Initializes a new instance of <c>PagedResult</c> class.
        /// </summary>
        /// <param name="totalRecords">Total number of records contained in the entire object set.</param>
        /// <param name="totalPages">Total number of pages.</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="data">The objects contained in the current page.</param>
        public PagedResult(int totalRecords, int totalPages, int pageSize, int pageNumber, IList<TAggregateRoot> entities)
        {
            this.totalRecords = totalRecords;
            this.totalPages = totalPages;
            this.pageSize = pageSize;
            this.pageNumber = pageNumber;
            this.entities = entities;
        }
        #endregion

        #region Public Properties
        private int totalRecords;
        /// <summary>
        /// Gets or sets the total number of the records.
        /// </summary>
        public int TotalRecords
        {
            get { return totalRecords; }
            set { totalRecords = value; }
        }

        private int totalPages;
        /// <summary>
        /// Gets or sets the total pages available.
        /// </summary>
        public int TotalPages
        {
            get { return totalPages; }
            set { totalPages = value; }
        }

        private int pageSize;
        /// <summary>
        /// Gets or sets the number of records for each page.
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        private int pageNumber;
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = value; }
        }

        private IList<TAggregateRoot> entities;
        /// <summary>
        /// Gets a list of objects contained by the current <c>PagedResult{TAggregateRoot}</c> object.
        /// </summary>
        public IEnumerable<TAggregateRoot> Entities
        {
            get { return entities; }
        }
        #endregion

        #region ICollection<TAggregateRoot> Members
        /// <summary>
        /// Adds an item to the System.Collections.Generic.ICollection{TAggregateRoot}.
        /// </summary>
        /// <param name="item">The object to add to the System.Collections.Generic.ICollection{TAggregateRoot}.</param>
        public void Add(TAggregateRoot item) => entities.Add(item);

        /// <summary>
        /// Removes all items from the System.Collections.Generic.ICollection{TAggregateRoot}.
        /// </summary>
        public void Clear() => entities.Clear();

        /// <summary>
        /// Determines whether the System.Collections.Generic.ICollection{TAggregateRoot} contains
        /// a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.ICollection{TAggregateRoot}.</param>
        /// <returns>true if item is found in the System.Collections.Generic.ICollection{TAggregateRoot}; otherwise,
        /// false.</returns>
        public bool Contains(TAggregateRoot item) => entities.Contains(item);

        /// <summary>
        /// Copies the elements of the System.Collections.Generic.ICollection{TAggregateRoot} to an
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements
        /// copied from System.Collections.Generic.ICollection{TAggregateRoot}. The System.Array must
        /// have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(TAggregateRoot[] array, int arrayIndex) => entities.CopyTo(array, arrayIndex);

        /// <summary>
        /// Gets the number of elements contained in the System.Collections.Generic.ICollection{TAggregateRoot}.
        /// </summary>
        public int Count => entities.Count;

        /// <summary>
        /// Gets a value indicating whether the System.Collections.Generic.ICollection{TAggregateRoot}
        /// is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Removes the first occurrence of a specific object from the System.Collections.Generic.ICollection{TAggregateRoot}.
        /// </summary>
        /// <param name="item">The object to remove from the System.Collections.Generic.ICollection{TAggregateRoot}.</param>
        /// <returns></returns>
        public bool Remove(TAggregateRoot item) => entities.Remove(item);

        #endregion

        #region IEnumerable<TAggregateRoot> Members
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A System.Collections.Generic.IEnumerator{TAggregateRoot} that can be used to iterate through
        /// the collection.</returns>
        public IEnumerator<TAggregateRoot> GetEnumerator() => entities.GetEnumerator();

        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An System.Collections.IEnumerator object that can be used to iterate through
        /// the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => entities.GetEnumerator();
        
        #endregion
    }

    /// <summary>
    /// Represents a collection which contains a set of objects that is from
    /// a specific page of the entire object set.
    /// </summary>
    /// <typeparam name="TAggregateRoot">The type of the aggregateRoot.</typeparam>
    public class PagedResult<TAggregateRoot> : PagedResult<Guid, TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot<Guid>, new()
    {
        /// <summary>
        /// Initializes a new instance of <c>PagedResult</c> class.
        /// </summary>
        /// <param name="totalRecords">Total number of records contained in the entire object set.</param>
        /// <param name="totalPages">Total number of pages.</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="data">The objects contained in the current page.</param>
        public PagedResult(int totalRecords, int totalPages, int pageSize, int pageNumber, IList<TAggregateRoot> entities)
            : base(totalRecords, totalPages, pageSize, pageNumber, entities)
        {

        }
    }
}
