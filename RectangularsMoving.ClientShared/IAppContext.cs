namespace RectangularsMoving.ClientShared {
    public interface IAppContext {
        Task RunInUiThreadAsync(Func<Task> action);
    }
}