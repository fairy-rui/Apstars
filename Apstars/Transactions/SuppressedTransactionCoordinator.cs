
namespace Apstars.Transactions
{
    /// <summary>
    /// Represents the transaction coordinator that does nothing else
    /// but simply commits the transactions for each managed <see cref="IUnitOfWork"/> instances.
    /// </summary>
    internal sealed class SuppressedTransactionCoordinator : TransactionCoordinator
    {
        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>SuppressedTransactionCoordinator</c> class.
        /// </summary>
        /// <param name="unitOfWorks">The <see cref="IUnitOfWork"/> instances to be managed by current
        /// transaction coordinator.</param>
        public SuppressedTransactionCoordinator(params IUnitOfWork[] unitOfWorks)
            : base(unitOfWorks) { }
        #endregion
    }
}
