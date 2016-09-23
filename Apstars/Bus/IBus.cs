﻿using System;
using System.Collections.Generic;

namespace Apstars.Bus
{
    /// <summary>
    /// Represents the message bus.
    /// </summary>
    public interface IBus : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Publishes the specified message to the bus.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message to be published.</typeparam>
        /// <param name="message">The message to be published.</param>
        void Publish<TMessage>(TMessage message);
        /// <summary>
        /// Publishes a collection of messages to the bus.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message to be published.</typeparam>
        /// <param name="messages">The messages to be published.</param>
        void Publish<TMessage>(IEnumerable<TMessage> messages);
        /// <summary>
        /// Clears the published messages waiting for commit.
        /// </summary>
        void Clear();
    }
}
