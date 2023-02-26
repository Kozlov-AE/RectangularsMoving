using Microsoft.Extensions.DependencyInjection;
using RectangularsMoving.ClientShared.ViewModels;
using RectangularsMoving.Protos;

namespace RectangularsMoving.ClientShared {
    public class SharedServices {
        private IServiceCollection _services;
        public SharedServices() {
            _services = new ServiceCollection();
        }

        public static IServiceCollection GerServices() {
            var services = new ServiceCollection();
            services.AddGrpcClient<RectMoving.RectMovingClient>(o => {
                o.Address = new Uri("https://localhost:7005");
            });
            services.AddSingleton<BoardViewModel>();
            services.AddSingleton<MainWindowViewModel>();

            return services;
        }
    }
}