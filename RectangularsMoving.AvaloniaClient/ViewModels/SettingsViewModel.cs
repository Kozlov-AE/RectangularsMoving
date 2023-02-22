using CommunityToolkit.Mvvm.ComponentModel;

namespace RectangularsMoving.AvaloniaClient.ViewModels {
    public partial class SettingsViewModel : ObservableObject {
        [ObservableProperty] private int _rectCount;
        [ObservableProperty] private int _boardHeight;
        [ObservableProperty] private int _boardWidth;
        [ObservableProperty] private int _taskCount;

        public SettingsViewModel() {
            RectCount = 100;
            BoardHeight = 500;
            BoardWidth = 500;
            TaskCount = 100;
        }
    }
}