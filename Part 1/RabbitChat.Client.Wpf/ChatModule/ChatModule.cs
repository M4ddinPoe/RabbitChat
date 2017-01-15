using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitChat.Client.Wpf.ChatModule
{
    using System.Windows.Controls;

    using Autofac;


    using Prism.Modularity;
    using Prism.Regions;

    using RabbitChat.Client.Wpf.ChatModule.Welcome;

    public class ChatModule : IModule
    {
        public ChatModule(IRegionManager regionManager, IContainer container)
        {
            this.RegionManager = regionManager;
            this.Container = container;
        }

        private IRegionManager RegionManager { get; }

        private IContainer Container { get; }

        public void Initialize()
        {
            var view = this.Container.Resolve<LoginView>();
            this.RegionManager.Regions["MainRegion"].Add(view);
        }
    }
}
