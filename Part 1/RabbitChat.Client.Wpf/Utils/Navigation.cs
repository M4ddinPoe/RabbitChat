namespace RabbitChat.Client.Wpf.Utils
{
    using Autofac;

    using Prism.Regions;

    /// <summary>
    /// Handles the navigation.
    /// </summary>
    public class Navigation : INavigation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Navigation"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="regionManager">The region manager.</param>
        public Navigation(IContainer container, IRegionManager regionManager)
        {
            this.Container = container;
            this.RegionManager = regionManager;
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        private IContainer Container { get; }

        /// <summary>
        /// Gets the region manager.
        /// </summary>
        /// <value>
        /// The region manager.
        /// </value>
        private IRegionManager RegionManager { get; }

        /// <summary>
        /// Navigates the specified region.
        /// </summary>
        /// <typeparam name="T">The type of the view.</typeparam>
        /// <param name="regionName">Name of the region.</param>
        public void Navigate<T>(string regionName = "MainRegion")
        {
            IRegion region = this.RegionManager.Regions[regionName];
            T view = this.Container.Resolve<T>();

            if (!region.Views.Contains(view))
            {
                region.Add(view);
                region.Activate(view);
            }
        }
    }
}