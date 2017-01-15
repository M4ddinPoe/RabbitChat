namespace RabbitChat.Client.Wpf.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Threading.Tasks;

    using MongoDB.Driver;

    using RabbitChat.Client.Wpf.Model;

    public class MongoDbRepository
    {
        private const string DatabaseName = "rabbitchat";

        private const string CollectionName = "contact";

        private static IMongoDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbRepository"/> class.
        /// </summary>
        public MongoDbRepository()
        {
            IMongoClient client = new MongoClient();
            database = client.GetDatabase(DatabaseName);
        }

        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// The generated Id
        /// </returns>
        public async Task<Guid> RegisterUser(Contact contact, SecureString password)
        {
            var generatedId = Guid.NewGuid();
            var insecurePassword = this.GetStringFromSecureString(password);

            contact.Id = generatedId;
            contact.Password = insecurePassword;

            var collection = database.GetCollection<Contact>(CollectionName);
            await collection.InsertOneAsync(contact);

            return generatedId;
        }

        /// <summary>
        /// Exists the user.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns><c>True</c> if user exists; otherwise <c>false</c>.</returns>
        public async Task<bool> ExistsUser(string email)
        {
            var collection = database.GetCollection<Contact>(CollectionName);
            var filter = this.CreateFilter("Email", email);

            var itemCount = await collection.CountAsync(filter);
            return itemCount > 0;
        }

        /// <summary>
        /// Exists the nickname.
        /// </summary>
        /// <param name="nickName">The nickname.</param>
        /// <returns><c>True</c> if nick name exists; otherwise <c>false</c>.</returns>
        public async Task<bool> ExistsNickname(string nickName)
        {
            var collection = database.GetCollection<Contact>(CollectionName);
            var filter = this.CreateFilter("NickName", nickName);

            var itemCount = await collection.CountAsync(filter);
            return itemCount > 0;
        }

        /// <summary>
        /// Logins the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>The contact of the logged in user.</returns>
        public async Task<Contact> Login(string email, SecureString password)
        {
            var collection = database.GetCollection<Contact>(CollectionName);
            var unsecurePassword = this.GetStringFromSecureString(password);

            var filter = this.CreateFilter("Email", email);
            filter &= this.CreateFilter("Password", unsecurePassword);

            var fields = Builders<Contact>.Projection.Exclude(c => c.Contacts);
            var contactsEnumerable = await this.GetDocumentsByFilter(collection, filter, fields);
            var contact = contactsEnumerable.FirstOrDefault();

            return contact;
        }

        /// <summary>
        /// Gets the contact by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The contact.</returns>
        public async Task<Contact> GetContact(string email)
        {
            var collection = database.GetCollection<Contact>(CollectionName);
            var filter = this.CreateFilter("Email", email);

            var fields = Builders<Contact>.Projection.Exclude(c => c.Contacts);
            var contactsEnumerable = await this.GetDocumentsByFilter(collection, filter, fields);
            var contact = contactsEnumerable.FirstOrDefault();

            return contact;
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
            var collection = database.GetCollection<Contact>(CollectionName);
            var filter = this.CreateFilter("_id", contact.Id);

            // todo: find a way to select sublist directly
            var fields = Builders<Contact>.Projection.Include(c => c.Contacts);
            var contactsEnumerable = await this.GetDocumentsByFilter(collection, filter, fields);

            return contactsEnumerable.First().Contacts;
        }

        /// <summary>
        /// Updates the contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns>The asynchronous task.</returns>
        public async Task UpdateContact(Contact contact)
        {
            var collection = database.GetCollection<Contact>(CollectionName);
            var filter = this.CreateFilter("_id", contact.Id);

            // todo: is there a better way??
            await collection.ReplaceOneAsync(filter, contact);
        }

        /// <summary>
        /// Adds the contact to list.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="contactToAdd">The contact to add.</param>
        /// <returns>The asynchronous task.</returns>
        public async Task AddContactToList(Contact contact, Contact contactToAdd)
        {
            var collection = database.GetCollection<Contact>(CollectionName);
            var filter = this.CreateFilter("_id", contact.Id);

            var update = Builders<Contact>.Update.Push(e => e.Contacts, contactToAdd);

            await collection.FindOneAndUpdateAsync(filter, update);
        }

        /// <summary>
        /// Gets a filter for collections.
        /// </summary>
        /// <param name="fieldName">Name of the field to filter.</param>
        /// <param name="filterValue">The filter value.</param>
        /// <returns>The requested filter.</returns>
        private FilterDefinition<Contact> CreateFilter(string fieldName, object filterValue)
        {
            var filter = Builders<Contact>.Filter.Eq(fieldName, filterValue);
            return filter;
        }

        /// <summary>
        /// Gets the documents in this collection by a given filter.
        /// </summary>
        /// <typeparam name="T">Type of the documents.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="fields">The fields.</param>
        /// <returns>
        /// The List of all found documents.
        /// </returns>
        private async Task<IEnumerable<T>> GetDocumentsByFilter<T>(
            IMongoCollection<T> collection, FilterDefinition<T> filter, ProjectionDefinition<T> fields = null)
        {
            var resultList = new List<T>();
            var options = new FindOptions<T>();

            if (fields != null)
            {
                options.Projection = fields;
            }

            using (var cursor = await collection.FindAsync(filter, options))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        resultList.Add(document);
                    }
                }
            }

            return resultList;
        }

        /// <summary>
        /// Gets the string from secure string.
        /// </summary>
        /// <param name="secureString">The secure string.</param>
        /// <returns>The unsecured string.</returns>
        private string GetStringFromSecureString(SecureString secureString)
        {
            string insecurePassword;

            try
            {
                var passwordBstr = Marshal.SecureStringToBSTR(secureString);
                insecurePassword = Marshal.PtrToStringBSTR(passwordBstr);
            }
            catch
            {
                insecurePassword = string.Empty;
            }

            return insecurePassword;
        }
    }
}
