namespace RabbitChat.Client.Wpf.Behaviour
{
    using System.Reflection;
    using System.Security;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Interactivity;

    public class PasswordBoxBindingBehavior : Behavior<PasswordBox>
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
            "Password",
            typeof(SecureString),
            typeof(PasswordBoxBindingBehavior),
            new PropertyMetadata(null));

        protected override void OnAttached()
        {
            this.AssociatedObject.PasswordChanged += this.OnPasswordBoxValueChanged;
        }

        public SecureString Password
        {
            get
            {
                return (SecureString)this.GetValue(PasswordProperty);
            }
            set
            {
                this.SetValue(PasswordProperty, value);
            }
        }

        private void OnPasswordBoxValueChanged(object sender, RoutedEventArgs e)
        {
            var binding = BindingOperations.GetBindingExpression(this, PasswordProperty);
            PropertyInfo property = binding?.DataItem.GetType().GetProperty(binding.ParentBinding.Path.Path);
            property?.SetValue(binding.DataItem, this.AssociatedObject.SecurePassword, null);
        }
    }
}
