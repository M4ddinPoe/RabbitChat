namespace RabbitChat.Client.Wpf.Utils
{
    using System;

    public interface IEventMessenger
    {
        /// <summary>
        /// Publishes the event.
        /// </summary>
        /// <typeparam name="T">The event type</typeparam>
        /// <param name="payload">The payload.</param>
        void PublishEvent<T>(T payload);

        /// <summary>
        /// Subscribes the event.
        /// </summary>
        /// <typeparam name="T">The event type.</typeparam>
        /// <param name="callback">The callback.</param>
        void SubscribeEvent<T>(Action<T> callback);
    }
}