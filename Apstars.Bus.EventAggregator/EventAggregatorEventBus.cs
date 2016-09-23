using Apstars.Events;

namespace Apstars.Bus.EventAggregator
{
    /// <summary>
    /// Represents the event bus implemented by using the event aggregator.
    /// </summary>
    public sealed class EventAggregatorEventBus : EventAggregatorBus, IEventBus
    {
        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>EventAggregatorEventBus</c> class.
        /// </summary>
        /// <param name="eventAggregator">The <see cref="IEventAggregator"/> instance.</param>
        public EventAggregatorEventBus(IEventAggregator eventAggregator)
            : base(eventAggregator) { }
        #endregion
    }
}
