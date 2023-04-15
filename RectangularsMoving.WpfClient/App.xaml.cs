using Microsoft.Extensions.DependencyInjection;
using RectangularsMoving.ClientShared;
using RectangularsMoving.ClientShared.ViewModels;
using RectangularsMoving.WpfClient.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RectangularsMoving.WpfClient {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public IServiceProvider Services { get; private set; }
        public App() {
            ConfigureServices();
        }

        private void OnStartup(object sender, StartupEventArgs startupEventArgs) {
            var mainWindow = new MainWindow();
            var mainViewModel = Services.GetRequiredService<MainWindowViewModel>();
            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();
        }

        private void ConfigureServices() {
            var services = SharedServices.GerServices();
            try {
                services.AddSingleton<IAppManager, AppManager>();
                Services = services.BuildServiceProvider();
            }
            catch (Exception e) {
                Services = new ServiceCollection().BuildServiceProvider();
            }
        }
    }
}
