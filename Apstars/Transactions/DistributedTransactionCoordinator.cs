using System.Transactions;

namespace Apstars.Transactions
{
    /// <summary>
    /// Represents the transaction coordinator that utilizes Microsoft Distributed
    /// Transaction Coordinator to control the transaction.
    /// </summary>
    internal sealed class DistributedTransactionCoordinator : TransactionCoordinator
    {
        #region Private Fields
        private readonly TransactionScope scope = new TransactionScope();
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>DistributedTransactionCoordinator</c> class.
        /// </summary>
        /// <param name="unitOfWorks">The <see cref="IUnitOfWork"/> instances to be managed by current
        /// transaction coordinator.</param>
        public DistributedTransactionCoordinator(params IUnitOfWork[] unitOfWorks)
            : base(unitOfWorks) { }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                scope.Dispose();
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public override void Commit()
        {
            base.Commit();
            scope.Complete();
        }
        #endregion
    }
}
