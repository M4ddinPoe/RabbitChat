namespace RabbitChat.Client.Wpf.ChatModule.Welcome
{
    using System.Security;
    using System.Windows;
    using System.Windows.Input;

    using Microsoft.Practices.Prism.Commands;
    using Microsoft.Practices.Prism.Mvvm;

    using RabbitChat.Client.Wpf.Model;
    using RabbitChat.Client.Wpf.Service;
    using RabbitChat.Client.Wpf.Utils;

    public class RegistrationViewModel : BindableBase
    {
        private string nickName;

        private string email;

        private SecureString password;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationViewModel" /> class.
        /// </summary>
        /// <param name="navigation">The navigation.</param>
        /// <param name="contactService">The contact service.</param>
        public RegistrationViewModel(INavigation navigation, ContactService contactService)
        {
            this.Navigation = navigation;
            this.ContactService = contactService;

            this.RegisterCommand = new DelegateCommand(this.OnRegister);
            this.CancelCommand = new DelegateCommand(this.OnCancel);
        }

        /// <summary>
        /// Gets the register command.
        /// </summary>
        /// <value>
        /// The register command.
        /// </value>
        public ICommand RegisterCommand { get; }

        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        /// <value>
        /// The cancel command.
        /// </value>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Gets or sets the name of the nick.
        /// </summary>
        /// <value>
        /// The name of the nick.
        /// </value>
        public string NickName
        {
            get
            {
                return this.nickName;
            }

            set
            {
                this.SetProperty(ref this.nickName, value);
            }
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email
        {
            get
            {
                return this.email;
            }

            set
            {
                this.SetProperty(ref this.email, value);
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
        /// Gets the navigation.
        /// </summary>
        /// <value>
        /// The navigation.
        /// </value>
        private INavigation Navigation { get; }

        /// <summary>
        /// Gets the contact service.
        /// </summary>
        /// <value>
        /// The contact service.
        /// </value>
        private ContactService ContactService { get; }

        /// <summary>
        /// Called when [register].
        /// </summary>
        private async void OnRegister()
        {
            var existsUser = await this.ContactService.ExistsUser(this.Email);

            if (existsUser)
            {
                this.ShowErrorMessage("This exists already.");
            }

            var existsNickname = await this.ContactService.ExistsNickname(this.NickName);

            if (existsNickname)
            {
                this.ShowErrorMessage("This nick name exists already.");
            }

            var contact = new Contact
                              {
                                  NickName = this.NickName,
                                  Email = this.Email,
                              };

            await this.ContactService.RegisterUser(contact, this.Password);
            this.Navigation.Navigate<LoginView>();
        }

        /// <summary>
        /// Called when [cancel].
        /// </summary>
        private void OnCancel()
        {
            this.Navigation.Navigate<LoginView>();
        }

        /// <summary>
        /// Shows the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void ShowErrorMessage(string message)
        {
            Application.Current.Dispatcher.Invoke(() => { MessageBox.Show(message); });
        }
    }
}
