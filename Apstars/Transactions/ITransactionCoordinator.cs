using System;

namespace Apstars.Transactions
{
    /// <summary>
    /// Represents that the implemented classes are transaction coordinators.
    /// </summary>
    public interface ITransactionCoordinator : IUnitOfWork, IDisposable
    {
    }
}
