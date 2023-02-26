using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using RectangularsMoving.AvaloniaClient.Views;
using RectangularsMoving.ClientShared;
using RectangularsMoving.ClientShared.ViewModels;
using System;

namespace RectangularsMoving.AvaloniaClient
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; private set; }

        public override void Initialize()
        {
            LoadServices();
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted() {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var vm = Services.GetRequiredService<MainWindowViewModel>();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = vm,
                };
                desktop.Exit += (sender, args) => {
                    vm.Stop();
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void LoadServices() {
            var services = SharedServices.GerServices();
            try {
                services.AddSingleton<IAppManager, AppManager>();

                Services = services.BuildServiceProvider();
            }
            catch (Exception ex) {
                Services = new ServiceCollection().BuildServiceProvider();
            }
        }
    }
}