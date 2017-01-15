namespace RabbitChat.Client.Wpf.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Threading.Tasks;
    using System.Windows.Documents;

    using MongoDB.Driver;

    using RabbitChat.Client.Wpf.Model;

    /// <summary>
    /// The contact repository.
    /// </summary>
    public class ContactRepository
    {
        public List<Contact> Contacts { get; } = new List<Contact>();

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
            return await Task.Run(
                () =>
                    {

                        var generatedId = Guid.NewGuid();
                        var insecurePassword = this.GetStringFromSecureString(password);

                        contact.Id = generatedId;
                        contact.Password = insecurePassword;

                        this.Contacts.Add(contact);

                        return generatedId;
                    });
        }

        /// <summary>
        /// Exists the user.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns><c>True</c> if user exists; otherwise <c>false</c>.</returns>
        public async Task<bool> ExistsUser(string email)
        {
            return await Task.Run(
                () =>
                    {
                        var itemCount = this.Contacts.Count(c => c.Email == email);
                        return itemCount > 0;
                    });
        }

        /// <summary>
        /// Exists the nickname.
        /// </summary>
        /// <param name="nickName">The nickname.</param>
        /// <returns><c>True</c> if nick name exists; otherwise <c>false</c>.</returns>
        public async Task<bool> ExistsNickname(string nickName)
        {
            return await Task.Run(
                () =>
                    {
                        var itemCount = this.Contacts.Count(c => c.NickName == nickName);
                        return itemCount > 0;
                    });
        }

        /// <summary>
        /// Logins the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>The contact of the logged in user.</returns>
        public async Task<Contact> Login(string email, SecureString password)
        {
            return await Task.Run(
                () =>
                    {
                        var unsecurePassword = this.GetStringFromSecureString(password);
                        var contact =
                            this.Contacts.FirstOrDefault(c => c.Email == email && c.Password == unsecurePassword);

                        return contact;
                    });
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
