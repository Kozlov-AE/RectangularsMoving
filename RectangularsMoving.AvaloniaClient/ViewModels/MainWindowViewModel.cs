using CommunityToolkit.Mvvm.ComponentModel;

namespace RectangularsMoving.AvaloniaClient.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject {
        public MainWindowViewModel() {
                Greeting = "Привет!";
        }
        [ObservableProperty]
        private string _greeting;
    }
}
