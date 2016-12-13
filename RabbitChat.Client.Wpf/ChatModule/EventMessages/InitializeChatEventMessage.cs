namespace RabbitChat.Client.Wpf.ChatModule.EventMessages
{
    using RabbitChat.Client.Wpf.Model;
    using RabbitChat.Client.Wpf.Service;

    public class InitializeChatEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeChatEventMessage"/> class.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public InitializeChatEventMessage(Contact contact)
        {
            this.Contact = contact;
        }

        /// <summary>
        /// Gets the contact.
        /// </summary>
        /// <value>
        /// The contact.
        /// </value>
        public Contact Contact { get; }

        /// <summary>
        /// Gets or sets the chat.
        /// </summary>
        /// <value>
        /// The chat.
        /// </value>
        public Chat Chat { get; set; }
    }
}
