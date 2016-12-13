namespace RabbitChat.Client.Wpf.Model
{
    public class Message
    {
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
