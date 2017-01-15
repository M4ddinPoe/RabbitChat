namespace RabbitChat.Client.Wpf.ChatModule.Welcome
{
    using System.Security;
    using System.Windows.Input;

    using Prism.Commands;
    using Prism.Mvvm;

    using RabbitChat.Client.Wpf.ChatModule.ContactList;
    using RabbitChat.Client.Wpf.ChatModule.EventMessages;
    using RabbitChat.Client.Wpf.Service;
    using RabbitChat.Client.Wpf.Utils;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Prism.Mvvm.BindableBase" />
    public class LoginViewModel : BindableBase
    {
        private string name;

        private SecureString password;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel" /> class.
        /// </summary>
        /// <param name="navigation">The navigation.</param>
        /// <param name="eventMessenger">The event messenger.</param>
        /// <param name="contactService">The contact service.</param>
        public LoginViewModel(INavigation navigation, IEventMessenger eventMessenger, ContactService contactService)
        {
            this.Navigation = navigation;
            this.EventMessenger = eventMessenger;
            this.ContactService = contactService;

            this.LoginCommand = new DelegateCommand(this.OnLogin);
            this.RegistrationCommand = new DelegateCommand(this.OnRegistration);
        }

        /// <summary>
        /// Gets the login command.
        /// </summary>
        /// <value>
        /// The login command.
        /// </value>
        public ICommand LoginCommand { get; }

        /// <summary>
        /// Gets the registration command.
        /// </summary>
        /// <value>
        /// The registration command.
        /// </value>
        public ICommand RegistrationCommand { get; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.SetProperty(ref this.name, value);
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public SecureString Password
        {
            get
            {
                return this.password;
            }

            set
            {
                this.SetProperty(ref this.password, value);
            }
        }

        /// <summary>
        /// Gets the contact service.
        /// </summary>
        /// <value>
        /// The contact service.
        /// </value>
        private ContactService ContactService { get; }

        /// <summary>
        /// Gets the navigation.
        /// </summary>
        /// <value>
        /// The navigation.
        /// </value>
        private INavigation Navigation { get; }

        /// <summary>
        /// Gets the event messenger.
        /// </summary>
        /// <value>
        /// The event messenger.
        /// </value>
        private IEventMessenger EventMessenger { get; }

        /// <summary>
        /// Called when logged in.
        /// </summary>
        private async void OnLogin()
        {
            var contact = await this.ContactService.Login(this.Name, this.Password);

            if (contact == null)
            {
                return;
            }

            this.Navigation.Navigate<ContactListView>();
            this.EventMessenger.PublishEvent(new LoginEventMessage(contact));
        }

        /// <summary>
        /// Called when [registration].
        /// </summary>
        private void OnRegistration()
        {
            this.Navigation.Navigate<RegistrationView>();
        }
    }
}
