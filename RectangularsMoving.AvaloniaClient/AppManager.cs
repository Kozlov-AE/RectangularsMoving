using RectangularsMoving.ClientShared;
using System;
using System.Threading.Tasks;

namespace RectangularsMoving.AvaloniaClient {
    public class AppManager :IAppManager {
        public Task RunInUiThreadAsync(Func<Task> action) {
            action();
            return Task.CompletedTask;
        }
    }
}