using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RectangularsMoving.AvaloniaClient.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject {
        public MainWindowViewModel() {
            IsConnectionNeeds = true;
        }

        [ObservableProperty] private bool _isConnectionNeeds;
        [ObservableProperty] private SettingsViewModel _settingsVm;

        [RelayCommand]
        private void Connect(SettingsViewModel vm) {
            IsConnectionNeeds = false;
        }
        [RelayCommand]
        private void Stop() {
            
        }
    }
}
