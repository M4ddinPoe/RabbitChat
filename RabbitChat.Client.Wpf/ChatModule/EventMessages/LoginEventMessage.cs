namespace RabbitChat.Client.Wpf.ChatModule.EventMessages
{
    using RabbitChat.Client.Wpf.Model;

    public class LoginEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginEventMessage"/> class.
        /// </summary>
        /// <param name="contact">The user number.</param>
        public LoginEventMessage(Contact contact)
        {
            this.Contact = contact;
        }

        /// <summary>
        /// Gets the user number.
        /// </summary>
        /// <value>
        /// The user number.
        /// </value>
        public Contact Contact { get; }
    }
}
