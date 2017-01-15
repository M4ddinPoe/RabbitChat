namespace RabbitChat.Client.Wpf.Utils
{
    /// <summary>
    /// Handles the navigation.
    /// </summary>
    public interface INavigation
    {
        /// <summary>
        /// Navigates the specified region.
        /// </summary>
        /// <typeparam name="T">The type of the view.</typeparam>
        /// <param name="regionName">Name of the region.</param>
        void Navigate<T>(string regionName = "MainRegion");
    }
}
