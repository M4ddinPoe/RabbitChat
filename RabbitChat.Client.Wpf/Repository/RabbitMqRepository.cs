namespace RabbitChat.Client.Wpf.Repository
{
    using System;

    using RabbitChat.Client.Wpf.Model;
    using RabbitMQ.Client;

    public class RabbitMqRepository
    {
        public RabbitMqRepository()
        {
            this.Factory = new ConnectionFactory() { HostName = "localhost" };
            this.Connection = this.Factory.CreateConnection();
        }

        /// <summary>
        /// Gets the factory.
        /// </summary>
        /// <value>
        /// The factory.
        /// </value>
        private ConnectionFactory Factory { get; }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        private IConnection Connection { get; }

        public void AddListener(string key, Action<Message> messageReceivedListener)
        {
            
        }

        public void AddPublisher()
        {
            
        }
    }
}
