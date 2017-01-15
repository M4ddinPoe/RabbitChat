namespace RabbitChat.Client.Wpf.ChatModule.Chat
{
    using Prism.Interactivity.InteractionRequest;

    using RabbitChat.Client.Wpf.Model;
    using RabbitChat.Client.Wpf.Service;

    /// <summary>
    /// The initialize chat notification.
    /// </summary>
    /// <seealso cref="Prism.Interactivity.InteractionRequest.Confirmation" />
    public class InitializeChatNotification : Confirmation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeChatNotification"/> class.
        /// </summary>
        public InitializeChatNotification()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeChatNotification"/> class.
        /// </summary>
        /// <param name="chat">The chat.</param>
        public InitializeChatNotification(Chat chat)
        {
            this.Chat = chat;
        }

        /// <summary>
        /// Gets or sets the chat.
        /// </summary>
        /// <value>
        /// The chat.
        /// </value>
        public Chat Chat { get; set; }

        /// <summary>
        /// Gets or sets the local user.
        /// </summary>
        /// <value>
        /// The local user.
        /// </value>
        public Contact LocalUser { get; set; }

        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>
        /// The contact.
        /// </value>
        public Contact Contact { get; set; }
    }
}
