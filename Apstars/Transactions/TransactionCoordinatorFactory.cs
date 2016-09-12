
namespace Apstars.Transactions
{
    /// <summary>
    /// Represents the factory type which creates an instance of <see cref="ITransactionCoordinator"/>
    /// based on the given unit of works.
    /// </summary>
    public static class TransactionCoordinatorFactory
    {
        #region Public Methods
        /// <summary>
        /// Creates an instance of <see cref="ITransactionCoordinator"/> based on the given unit of works.
        /// </summary>
        /// <param name="args">The unit of works.</param>
        /// <returns>An instance of <see cref="ITransactionCoordinator"/> type.</returns>
        public static ITransactionCoordinator Create(params IUnitOfWork[] args)
        {
            bool ret = true;
            foreach (var arg in args)
                ret = ret && arg.DistributedTransactionSupported;
            if (ret)
                return new DistributedTransactionCoordinator(args);
            else
                return new SuppressedTransactionCoordinator(args);
        }
        #endregion
    }
}
