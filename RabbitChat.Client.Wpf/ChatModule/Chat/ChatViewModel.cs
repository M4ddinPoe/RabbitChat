namespace RabbitChat.Client.Wpf.ChatModule.Chat
{
    using System;

    using System.Windows.Input;

    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;

    using Prism.Commands;

    using RabbitChat.Client.Wpf.Model;
    using RabbitChat.Client.Wpf.Service;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.Practices.Prism.Mvvm.BindableBase" />
    public class ChatViewModel : BindableBase, IInteractionRequestAware
    {
        private string messages;

        private string messageToSend;

        private InitializeChatNotification initializeChatNotification;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatViewModel" /> class.
        /// </summary>
        /// <param name="rabbitChatService">The rabbit chat service.</param>
        public ChatViewModel(RabbitChatService rabbitChatService)
        {
            this.RabbitChatService = rabbitChatService;
            this.SendCommand = new DelegateCommand(this.OnSendMessage);
            this.CloseCommand = new DelegateCommand(this.OnClose);
        }

        /// <summary>
        /// Gets the send command.
        /// </summary>
        /// <value>
        /// The send command.
        /// </value>
        public ICommand SendCommand { get; }

        /// <summary>
        /// Gets the close command.
        /// </summary>
        /// <value>
        /// The close command.
        /// </value>
        public ICommand CloseCommand { get; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public string Messages
        {
            get
            {
                return this.messages;
            }

            set
            {
                this.SetProperty(ref this.messages, value);
            }
        }

        /// <summary>
        /// Gets or sets the message to send.
        /// </summary>
        /// <value>
        /// The message to send.
        /// </value>
        public string MessageToSend
        {
            get
            {
                return this.messageToSend;
            }

            set
            {
                this.SetProperty(ref this.messageToSend, value);
            }
        }

        /// <summary>
        /// Gets the rabbit chat service.
        /// </summary>
        /// <value>
        /// The rabbit chat service.
        /// </value>
        private RabbitChatService RabbitChatService { get; }

        /// <summary>
        /// Gets the chat.
        /// </summary>
        /// <value>
        /// The chat.
        /// </value>
        private Chat Chat { get; set; }

        /// <summary>
        /// The <see cref="T:Prism.Interactivity.InteractionRequest.INotification" /> passed when the interaction request was raised.
        /// </summary>
        public INotification Notification
        {
            get
            {
                return this.initializeChatNotification;
            }

            set
            {
                var notification = value as InitializeChatNotification;

                if (notification != null)
                {
                    this.initializeChatNotification = notification;
                    this.OnPropertyChanged(() => this.Notification);

                    this.Chat = notification.Chat;
                    this.Chat.MessageReceived += this.ChatOnMessageReceived;
                }
            }
        }

        /// <summary>
        /// An <see cref="T:System.Action" /> that can be invoked to finish the interaction.
        /// </summary>
        public Action FinishInteraction { get; set; }

        /// <summary>
        /// Called when message is send.
        /// </summary>
        private void OnSendMessage()
        {
            var message = new Message(this.RabbitChatService.CurrentUser, this.MessageToSend);
            this.Chat.SendMessage(message);
            this.AddMessageToFlow(this.RabbitChatService.CurrentUser.NickName, this.MessageToSend);

            this.MessageToSend = string.Empty;
        }

        /// <summary>
        /// Adds the message to flow.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        private void AddMessageToFlow(string name, string message)
        {
            this.Messages += $"{name}: {message}{Environment.NewLine}";
        }

        /// <summary>
        /// Chats the on message received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        private void ChatOnMessageReceived(object sender, Message message)
        {
            this.AddMessageToFlow(message.Contact.NickName, message.MessageText);
        }

        /// <summary>
        /// Raises the Close event.
        /// </summary>
        private void OnClose()
        {
            this.RabbitChatService.CloseChat(this.Chat);
            this.Chat = null;
        }
    }
}
