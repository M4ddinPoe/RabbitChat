namespace RabbitChat.Client.Wpf.ChatModule.ContactList
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    using Microsoft.Practices.Prism.Mvvm;

    using Prism.Commands;
    using Prism.Interactivity.InteractionRequest;

    using RabbitChat.Client.Wpf.ChatModule.Chat;
    using RabbitChat.Client.Wpf.ChatModule.EventMessages;
    using RabbitChat.Client.Wpf.Model;
    using RabbitChat.Client.Wpf.Service;
    using RabbitChat.Client.Wpf.Utils;

    public class ContactListViewModel : BindableBase
    {
        private Contact selectedContact;

        private string emailToAdd;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactListViewModel" /> class.
        /// </summary>
        /// <param name="eventMessenger">The event messenger.</param>
        /// <param name="rabbitChatService">The rabbit chat service.</param>
        /// <param name="contactService">The contact service.</param>
        public ContactListViewModel(IEventMessenger eventMessenger, RabbitChatService rabbitChatService, ContactService contactService)
        {
            this.EventMessenger = eventMessenger;
            this.RabbitChatService = rabbitChatService;
            this.ContactService = contactService;

            this.SelectedItemChangedCommand = new DelegateCommand<Contact>(this.OnSelectedItemChanged);
            this.ListDoubleClickedCommand = new DelegateCommand(this.OnListDoubleClicked);
            this.AddContactCommand = new DelegateCommand(this.OnAddContact);

            this.InitializeChatNotificationRequest = new InteractionRequest<InitializeChatNotification>();

            this.EventMessenger.SubscribeEvent<LoginEventMessage>(this.OnLogin);
        }

        /// <summary>
        /// Gets the selected item changed command.
        /// </summary>
        /// <value>
        /// The selected item changed command.
        /// </value>
        public ICommand SelectedItemChangedCommand { get; }

        /// <summary>
        /// Gets the list double clicked command.
        /// </summary>
        /// <value>
        /// The list double clicked command.
        /// </value>
        public ICommand ListDoubleClickedCommand { get; }

        /// <summary>
        /// Gets the add contact command.
        /// </summary>
        /// <value>
        /// The add contact command.
        /// </value>
        public ICommand AddContactCommand { get; }

        /// <summary>
        /// Gets the contacts.
        /// </summary>
        /// <value>
        /// The contacts.
        /// </value>
        public ObservableCollection<Contact> Contacts { get; } = new ObservableCollection<Contact>();

        /// <summary>
        /// Gets or sets the email to add.
        /// </summary>
        /// <value>
        /// The email to add.
        /// </value>
        public string EmailToAdd
        {
            get
            {
                return this.emailToAdd;
            }

            set
            {
                this.SetProperty(ref this.emailToAdd, value);
            }
        }

        /// <summary>
        /// Gets the selected contact.
        /// </summary>
        /// <value>
        /// The selected contact.
        /// </value>
        public Contact SelectedContact
        {
            get
            {
                return this.selectedContact;
            }

            private set
            {
                this.SetProperty(ref this.selectedContact, value);
            }
        }

        /// <summary>
        /// Gets the initialize chat notification request.
        /// </summary>
        /// <value>
        /// The initialize chat notification request.
        /// </value>
        public InteractionRequest<InitializeChatNotification> InitializeChatNotificationRequest { get; }

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
        /// Gets the contact service.
        /// </summary>
        /// <value>
        /// The contact service.
        /// </value>
        private ContactService ContactService { get; }

        /// <summary>
        /// Called when [login].
        /// </summary>
        /// <param name="loginEventMessage">The login event message.</param>
        private void OnLogin(LoginEventMessage loginEventMessage)
        {
            this.RabbitChatService.Initialize(loginEventMessage.Contact);
            this.RabbitChatService.ChatCreated += this.RabbitChatServiceOnChatCreated;

            this.LoadContacts();
        }

        /// <summary>
        /// Loads the contacts.
        /// </summary>
        private async void LoadContacts()
        {
            var contacts = await this.ContactService.GetContacts(this.RabbitChatService.CurrentUser);
            this.OnContactsLoaded(contacts);
        }

        /// <summary>
        /// Called when [contacts loaded].
        /// </summary>
        /// <param name="contacts">The contacts.</param>
        private void OnContactsLoaded(IEnumerable<Contact> contacts)
        {
            Application.Current.Dispatcher.Invoke(
                () =>
                    {
                        this.Contacts.AddRange(contacts);
                    });
        }

        /// <summary>
        /// Rabbits the chat service on chat created.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="chat">The chat.</param>
        private void RabbitChatServiceOnChatCreated(object sender, Chat chat)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(
                    () =>
                        {
                            this.InitializeChatNotificationRequest.Raise(new InitializeChatNotification(chat) { Title = chat.Contact.NickName });
                        });
                
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }
        }

        /// <summary>
        /// Called when [selected item changed].
        /// </summary>
        /// <param name="contact">The selected contact.</param>
        private void OnSelectedItemChanged(Contact contact)
        {
            this.SelectedContact = contact;
        }

        /// <summary>
        /// Called when [list double clicked].
        /// </summary>
        private void OnListDoubleClicked()
        {
            if (this.SelectedContact == null)
            {
                return;
            }

            var contact = this.SelectedContact;
            this.RabbitChatService.CreateChat(contact);
        }

        /// <summary>
        /// Called when [add contact].
        /// </summary>
        private async void OnAddContact()
        {
            if (await this.ContactService.ExistsUser(this.EmailToAdd))
            {
                var contact = await this.ContactService.GetContact(this.EmailToAdd);
                this.RabbitChatService.CurrentUser.Contacts.Add(contact);
                await this.ContactService.AddContactToList(this.RabbitChatService.CurrentUser, contact);

                this.OnContactAdded(contact);
            }
        }

        /// <summary>
        /// Called when [contact added].
        /// </summary>
        /// <param name="contact">The contact.</param>
        private void OnContactAdded(Contact contact)
        {
            Application.Current.Dispatcher.Invoke(
                () =>
                    {
                        this.EmailToAdd = string.Empty;
                        this.Contacts.Add(contact);
                    });
        }
    }
}
