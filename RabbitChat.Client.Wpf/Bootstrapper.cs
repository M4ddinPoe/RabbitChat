namespace RabbitChat.Client.Wpf
{
    using System;
    using System.Windows;

    using Autofac;

    using Microsoft.Practices.ServiceLocation;

    using Prism.Autofac;
    using Prism.Modularity;

    using RabbitChat.Client.Wpf.ChatModule.Chat;
    using RabbitChat.Client.Wpf.ChatModule.ContactList;
    using RabbitChat.Client.Wpf.ChatModule.Welcome;
    using RabbitChat.Client.Wpf.Repository;
    using RabbitChat.Client.Wpf.Service;
    using RabbitChat.Client.Wpf.Utils;

    /// <summary>
    /// Bootstraps the project.
    /// </summary>
    /// <seealso cref="Prism.Autofac.AutofacBootstrapper" />
    public class Bootstrapper : AutofacBootstrapper
    {
        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>
        /// The shell of the application.
        /// </returns>
        /// <remarks>
        /// If the returned instance is a <see cref="T:System.Windows.DependencyObject" />, the
        /// <see cref="T:Prism.Bootstrapper" /> will attach the default <see cref="T:Prism.Regions.IRegionManager" /> of
        /// the application in its <see cref="F:Prism.Regions.RegionManager.RegionManagerProperty" /> attached property
        /// in order to be able to add regions by using the <see cref="F:Prism.Regions.RegionManager.RegionNameProperty" />
        /// attached property from XAML.
        /// </remarks>
        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<Shell>();
        }

        /// <summary>
        /// Initializes the shell.
        /// </summary>
        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }

        /// <summary>
        /// Configures the <see cref="T:Autofac.ContainerBuilder" />.
        /// May be overwritten in a derived class to add specific type mappings required by the application.
        /// </summary>
        /// <param name="builder">The IoC container builder.</param>
        protected override void ConfigureContainerBuilder(ContainerBuilder builder)
        {
            base.ConfigureContainerBuilder(builder);

            builder.RegisterType<Shell>();

            builder.RegisterType<Navigation>().As<INavigation>();
            builder.RegisterType<EventMessenger>().As<IEventMessenger>();

            builder.RegisterType<ChatView>();
            builder.RegisterType<ChatViewModel>();
            builder.RegisterType<LoginView>();
            builder.RegisterType<LoginViewModel>();
            builder.RegisterType<RegistrationView>();
            builder.RegisterType<RegistrationViewModel>();
            builder.RegisterType<ContactListView>();
            builder.RegisterType<ContactListViewModel>();

            builder.RegisterType<RabbitChatService>().InstancePerLifetimeScope();
            builder.RegisterType<ContactService>();
            builder.RegisterType<MongoDbRepository>();
        }

        /// <summary>
        /// Configures the <see cref="T:Prism.Modularity.IModuleCatalog" /> used by Prism.
        /// </summary>
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            this.AddModule<ChatModule.ChatModule>();
        }

        /// <summary>
        /// Adds the module.
        /// </summary>
        /// <typeparam name="T">The type of the IModule.</typeparam>
        protected void AddModule<T>() where T : IModule
        {
            var moduleType = typeof(T);

            var moduleInfo = new ModuleInfo
            {
                ModuleName = moduleType.Name,
                ModuleType = moduleType.AssemblyQualifiedName,
                Ref = new Uri(moduleType.Assembly.CodeBase, UriKind.RelativeOrAbsolute).AbsoluteUri
            };

            this.ModuleCatalog.AddModule(moduleInfo);
        }
    }
}
