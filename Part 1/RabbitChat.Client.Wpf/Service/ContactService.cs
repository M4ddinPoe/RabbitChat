namespace RabbitChat.Client.Wpf.Service
{
    using System;
    using System.Collections.Generic;
    using System.Security;
    using System.Threading.Tasks;

    using RabbitChat.Client.Wpf.Model;
    using RabbitChat.Client.Wpf.Repository;

    public class ContactService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactService"/> class.
        /// </summary>
        /// <param name="mongoDbRepository">The mongo database repository.</param>
        public ContactService(MongoDbRepository mongoDbRepository)
        {
            this.MongoDbRepository = mongoDbRepository;
        }

        /// <summary>
        /// Gets the mongo database repository.
        /// </summary>
        /// <value>
        /// The mongo database repository.
        /// </value>
        private MongoDbRepository MongoDbRepository { get; }

        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// The user number.
        /// </returns>
        public async Task<Guid> RegisterUser(Contact contact, SecureString password)
        {
            return await this.MongoDbRepository.RegisterUser(contact, password);
        }

        /// <summary>
        /// Logins the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>The contact of the logged in user.</returns>
        public async Task<Contact> Login(string email, SecureString password)
        {
            return await this.MongoDbRepository.Login(email, password);
        }

        /// <summary>
        /// Exists the user with email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns><c>True</c> if user exists; otherwise <c>false</c></returns>
        public async Task<bool> ExistsUser(string email)
        {
            return await this.MongoDbRepository.ExistsUser(email);
        }

        /// <summary>
        /// Exists the nickname.
        /// </summary>
        /// <param name="nickName">The nickname.</param>
        /// <returns><c>True</c> if nick name exists; otherwise <c>false</c>.</returns>
        public async Task<bool> ExistsNickname(string nickName)
        {
            return await this.MongoDbRepository.ExistsNickname(nickName);
        }

        /// <summary>
        /// Gets the contact by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The contact.</returns>
        public async Task<Contact> GetContact(string email)
        {
            return await this.MongoDbRepository.GetContact(email);
        }

        /// <summary>
        /// Gets the contacts.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns>
        /// The contacts.
        /// </returns>
        public async Task<IEnumerable<Contact>> GetContacts(Contact contact)
        {
            return await this.MongoDbRepository.GetContacts(contact);
        }

        /// <summary>
        /// Adds the contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns>The asynchronous Task.</returns>
        public async Task UpdateContact(Contact contact)
        {
            await this.MongoDbRepository.UpdateContact(contact);
        }

        /// <summary>
        /// Adds the contact to list.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="contactToAdd">The contact to add.</param>
        /// <returns>The asynchronous task.</returns>
        public async Task AddContactToList(Contact contact, Contact contactToAdd)
        {
            await this.MongoDbRepository.AddContactToList(contact, contactToAdd);
        }
    }
}
