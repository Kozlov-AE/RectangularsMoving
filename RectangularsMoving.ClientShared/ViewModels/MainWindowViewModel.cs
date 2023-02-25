using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grpc.Net.Client;
using RectangularsMoving.Shared.Protos;

namespace RectangularsMoving.ClientShared.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject {

        [ObservableProperty] private bool _isConnectionNeeds;
        [ObservableProperty] private SettingsViewModel _settingsVm;
        [ObservableProperty] private BoardViewModel _boardVm;
        public MainWindowViewModel(BoardViewModel boardVm) {
            _boardVm = boardVm;
            IsConnectionNeeds = true;
            SettingsVm = new SettingsViewModel();
        }

        [RelayCommand]
        private async Task Connect(SettingsViewModel vm) {
            IsConnectionNeeds = false;
            BoardVm.SetBoardsProperties(vm.BoardWidth, vm.BoardHeight);
            try {
                var channel = GrpcChannel.ForAddress("https://localhost:7005");
                var client = new RectMoving.RectMovingClient(channel);
                var request = new ConfigRequest();
                request.TasksCount = vm.TaskCount;
                request.TaskDelay = vm.TaskDelay;
                request.Board = new Board() {
                    Height = vm.BoardHeight, Width = vm.BoardWidth, RectsCount = vm.RectCount
                };

                using var call = client.SetConfig(request);
                while (await call.ResponseStream.MoveNext(CancellationToken.None)) {
                    Task.Run(() => {
                        BoardVm.SetRectCoords(call.ResponseStream.Current);
                    });
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }
        [RelayCommand]
        private void Stop() {
            
        }
    }
}
