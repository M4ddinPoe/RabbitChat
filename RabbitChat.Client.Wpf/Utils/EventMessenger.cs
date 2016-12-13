namespace RabbitChat.Client.Wpf.Utils
{
    using System;

    using Prism.Events;

    /// <summary>
    /// The event messenger.
    /// </summary>
    public class EventMessenger : IEventMessenger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventMessenger"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        public EventMessenger(IEventAggregator eventAggregator)
        {
            this.EventAggregator = eventAggregator;
        }

        /// <summary>
        /// Gets the event aggregator.
        /// </summary>
        /// <value>
        /// The event aggregator.
        /// </value>
        private IEventAggregator EventAggregator { get; }

        /// <summary>
        /// Publishes the event.
        /// </summary>
        /// <typeparam name="T">The event type</typeparam>
        /// <param name="payload">The payload.</param>
        public void PublishEvent<T>(T payload)
        {
            this.EventAggregator.GetEvent<PubSubEvent<T>>().Publish(payload);
        }

        /// <summary>
        /// Subscribes the event.
        /// </summary>
        /// <typeparam name="T">The event type.</typeparam>
        /// <param name="callback">The callback.</param>
        public void SubscribeEvent<T>(Action<T> callback)
        {
            this.EventAggregator.GetEvent<PubSubEvent<T>>().Subscribe(callback);
        }
    }
}
