namespace RabbitChat.Client.Wpf.ChatModule.Chat
{
    using System;

    using System.Windows.Input;

    using Microsoft.Practices.Prism.Mvvm;

    using Prism.Commands;

    using RabbitChat.Client.Wpf.ChatModule.EventMessages;
    using RabbitChat.Client.Wpf.Model;
    using RabbitChat.Client.Wpf.Service;
    using RabbitChat.Client.Wpf.Utils;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.Practices.Prism.Mvvm.BindableBase" />
    public class ChatViewModel : BindableBase
    {
        private string messages;

        private string messageToSend;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatViewModel" /> class.
        /// </summary>
        /// <param name="eventMessenger">The event messenger.</param>
        /// <param name="rabbitChatService">The rabbit chat service.</param>
        public ChatViewModel(IEventMessenger eventMessenger, RabbitChatService rabbitChatService)
        {
            this.EventMessenger = eventMessenger;
            this.RabbitChatService = rabbitChatService;
            this.SendCommand = new DelegateCommand(this.OnSendMessage);
            this.CloseCommand = new DelegateCommand(this.OnClose);

            this.EventMessenger.SubscribeEvent<InitializeChatEventMessage>(this.OnInitializeChat);
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
        /// Gets the event messenger.
        /// </summary>
        /// <value>
        /// The event messenger.
        /// </value>
        private IEventMessenger EventMessenger { get; }

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
        /// Called when [initialize chat].
        /// </summary>
        /// <param name="initializeChatEventMessage">The initialize chat event message.</param>
        private void OnInitializeChat(InitializeChatEventMessage initializeChatEventMessage)
        {
            if (this.Chat != null)
            {
                return;
            }

            this.Chat = initializeChatEventMessage.Chat;
            this.Chat.MessageReceived += this.ChatOnMessageReceived;
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
