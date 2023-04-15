using RectangularsMoving.ClientShared;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RectangularsMoving.WpfClient {
    public class AppManager : IAppManager {
        public async Task RunInUiThreadAsync(Func<Task> func) {
            await Application.Current.Dispatcher.Invoke(func);
        }
    }
}