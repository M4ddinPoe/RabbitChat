namespace RabbitChat.Client.Wpf.ChatModule.Chat
{
    using Prism.Interactivity.InteractionRequest;

    using RabbitChat.Client.Wpf.Model;
    using RabbitChat.Client.Wpf.Service;

    public class InitializeChatNotification : Confirmation
    {
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
