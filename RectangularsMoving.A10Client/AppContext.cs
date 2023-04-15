using Avalonia.Threading;
using RectangularsMoving.ClientShared;
using System;
using System.Threading.Tasks;

namespace RectangularsMoving.A10Client {
    public class AppContext :IAppContext {
        public Task RunInUiThreadAsync(Func<Task> action) {
            return Dispatcher.UIThread.InvokeAsync(action);
        }
    }
}