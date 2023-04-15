using Avalonia.Threading;
using RectangularsMoving.ClientShared;
using System;
using System.Threading.Tasks;

namespace RectangularsMoving.AvaloniaClient {
    public class AppContext :IAppContext {
        public async Task RunInUiThreadAsync(Func<Task> action) {
            // Avalonia 11 allowed change binded parameters from different threads.
            // But I've found not expected behavior -
            // if we change many parameters, it render each of them
            //
            //action();
            //return Task.CompletedTask;
            await Dispatcher.UIThread.InvokeAsync(action);
        }
    }
}