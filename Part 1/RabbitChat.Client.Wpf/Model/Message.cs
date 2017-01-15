namespace RabbitChat.Client.Wpf.Model
{
    /// <summary>
    /// Represents a chat message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="messageText">The message text.</param>
        public Message(Contact contact, string messageText)
        {
            this.Contact = contact;
            this.MessageText = messageText;
        }

        /// <summary>
        /// Gets the contact.
        /// </summary>
        /// <value>
        /// The contact.
        /// </value>
        public Contact Contact { get; }

        /// <summary>
        /// Gets the message text.
        /// </summary>
        /// <value>
        /// The message text.
        /// </value>
        public string MessageText { get; }
    }
}
