using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Apstars.Transactions
{
    /// <summary>
    /// Represents the base class for transaction coordinators.
    /// </summary>
    public abstract class TransactionCoordinator : DisposableObject, ITransactionCoordinator
    {
        #region Private Fields
        private readonly List<IUnitOfWork> managedUnitOfWorks = new List<IUnitOfWork>();
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>TransactionCoordinator</c> class.
        /// </summary>
        /// <param name="unitOfWorks">The <see cref="IUnitOfWork"/> instances to be managed by current
        /// transaction coordinator.</param>
        public TransactionCoordinator(params IUnitOfWork[] unitOfWorks)
        {
            if (unitOfWorks == null ||
                unitOfWorks.Length == 0)
                throw new ArgumentNullException("unitOfWorks");
            this.managedUnitOfWorks.AddRange(unitOfWorks);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing) { }
        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work could support Microsoft Distributed
        /// Transaction Coordinator (MS-DTC).
        /// </summary>
        public virtual bool DistributedTransactionSupported
        {
            get { return false; }
        }
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work was successfully committed.
        /// </summary>
        public virtual bool Committed
        {
            get { return true; }
        }
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public virtual void Commit()
        {
            foreach (var uow in managedUnitOfWorks)
                uow.Commit();
        }

        public virtual Task CommitAsync()
        {
            return CommitAsync(CancellationToken.None);
        }

        public virtual async Task CommitAsync(CancellationToken cancellationToken)
        {
            foreach (var uow in managedUnitOfWorks)
                await uow.CommitAsync(cancellationToken);
        }
        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public virtual void Rollback() { }

        #endregion
    }
}
