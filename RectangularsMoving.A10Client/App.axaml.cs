using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using RectangularsMoving.A10Client.Views;
using RectangularsMoving.ClientShared;
using RectangularsMoving.ClientShared.ViewModels;
using System;

namespace RectangularsMoving.A10Client {
    public partial class App : Application {
        private IServiceProvider _services;
        public override void Initialize() {
            LoadServices();
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted() {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
                var vm = _services.GetRequiredService<MainWindowViewModel>();
                desktop.MainWindow = new MainWindow();
                desktop.MainWindow.DataContext = vm;
                desktop.Exit += (sender, args) => vm.Stop();
            }

            base.OnFrameworkInitializationCompleted();
        }
        
        private void LoadServices() {
            var services = SharedServices.GerServices();
                
            try {
                services.AddSingleton<IAppContext, AppContext>();

                _services = services.BuildServiceProvider();
            }
            catch (Exception ex) {
                _services = new ServiceCollection().BuildServiceProvider();
            }
        }

    }
}