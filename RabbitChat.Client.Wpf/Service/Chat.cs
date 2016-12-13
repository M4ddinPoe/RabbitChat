namespace RabbitChat.Client.Wpf.Service
{
    using System;
    using System.Text;

    using Newtonsoft.Json;

    using RabbitChat.Client.Wpf.Model;

    using RabbitMQ.Client;

    public class Chat : IDisposable
    {
        public Chat(IConnection connection, Contact contact)
        {
            this.Contact = contact;
            this.Connection = connection;
            this.SendChannel = this.Connection.CreateModel();

            this.SendChannel.ExchangeDeclare(
                exchange: "topic_chat", 
                type: "topic");
        }

        /// <summary>
        /// Occurs when [message received].
        /// </summary>
        public event EventHandler<Message> MessageReceived;

        public Contact Contact { get; }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        private IConnection Connection { get; }

        /// <summary>
        /// Gets the receive channel.
        /// </summary>
        /// <value>
        /// The receive channel.
        /// </value>
        private IModel SendChannel { get; }

        /// <summary>
        /// Called when [message received].
        /// </summary>
        /// <param name="message">The message.</param>
        public void OnMessageReceived(Message message)
        {
            this.MessageReceived?.Invoke(this, message);
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SendMessage(Message message)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            this.SendChannel.BasicPublish(exchange: "topic_chat", routingKey: "chat." + this.Contact.Id, basicProperties: null, body: body);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            this.SendChannel.Close();
            this.SendChannel.Dispose();
        }
    }
}
