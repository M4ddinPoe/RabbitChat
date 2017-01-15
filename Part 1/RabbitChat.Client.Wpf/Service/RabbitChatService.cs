namespace RabbitChat.Client.Wpf.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using RabbitChat.Client.Wpf.Model;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    /// <summary>
    /// The Rabbit Chat service.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class RabbitChatService : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitChatService" /> class.
        /// </summary>
        public RabbitChatService()
        {
            this.Factory = new ConnectionFactory() { HostName = "localhost" };
            this.Connection = this.Factory.CreateConnection();
            this.ReceiveChannel = this.Connection.CreateModel();
        }

        /// <summary>
        /// Occurs when [chat created].
        /// </summary>
        public event EventHandler<Chat> ChatCreated; 

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <value>
        /// The current user.
        /// </value>
        public Contact CurrentUser { get; private set; }

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

        /// <summary>
        /// Gets the receive channel.
        /// </summary>
        /// <value>
        /// The receive channel.
        /// </value>
        private IModel ReceiveChannel { get; }

        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        private string QueueName { get; set; }

        /// <summary>
        /// Gets the active chates.
        /// </summary>
        /// <value>
        /// The active chates.
        /// </value>
        private List<Chat> ActiveChats { get; } = new List<Chat>();

        /// <summary>
        /// Initializes the specified contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public void Initialize(Contact contact)
        {
            this.CurrentUser = contact;

            this.ReceiveChannel.ExchangeDeclare(
                exchange: "topic_chat", 
                type: "topic");

            this.QueueName = this.ReceiveChannel.QueueDeclare().QueueName;

            this.ReceiveChannel.QueueBind(queue: this.QueueName,
                                  exchange: "topic_chat",
                                  routingKey: "chat." + this.CurrentUser.Id);

            this.ReceiveMessagesAsync();
        }

        /// <summary>
        /// Creates the chat.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Chat CreateChat(Contact contact)
        {
            var chat = new Chat(this.Connection, contact);
            this.ActiveChats.Add(chat);
            this.ChatCreated?.Invoke(this, chat);

            return chat;
        }

        /// <summary>
        /// Closes the chat.
        /// </summary>
        /// <param name="chat">The chat.</param>
        public void CloseChat(Chat chat)
        {
            this.ActiveChats.Remove(chat);
            chat.Dispose();
        }

        /// <summary>
        /// Receives the messages asynchronous.
        /// </summary>
        private async void ReceiveMessagesAsync()
        {
            await Task.Run(() => this.ReceiveMessages());
        }

        /// <summary>
        /// Receives the messages.
        /// </summary>
        private void ReceiveMessages()
        {
            var consumer = new EventingBasicConsumer(this.ReceiveChannel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var bodyString = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<Message>(bodyString);

                var chat = this.ActiveChats.FirstOrDefault(c => c.Contact.Id == message.Contact.Id)
                           ?? this.CreateChat(message.Contact);

                chat.OnMessageReceived(message);
                Console.WriteLine(" [x] Received {0}", message);
            };
            this.ReceiveChannel.BasicConsume(queue: this.QueueName, noAck: true, consumer: consumer);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.ReceiveChannel.Dispose();
            this.Connection.Dispose();
        }
    }
}
