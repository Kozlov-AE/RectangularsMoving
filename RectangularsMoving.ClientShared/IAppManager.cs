namespace RectangularsMoving.ClientShared {
    public interface IAppManager {
        Task RunInUiThreadAsync(Func<Task> action);
    }
}