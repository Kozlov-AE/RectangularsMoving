using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grpc.Net.Client;
using RectangularsMoving.Shared.Protos;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RectangularsMoving.AvaloniaClient.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject {
        public MainWindowViewModel() {
            IsConnectionNeeds = true;
            SettingsVm = new SettingsViewModel();
        }

        [ObservableProperty] private bool _isConnectionNeeds;
        [ObservableProperty] private SettingsViewModel _settingsVm;
        [ObservableProperty] private BoardViewModel _boardVm;

        [RelayCommand]
        private async Task Connect(SettingsViewModel vm) {
            IsConnectionNeeds = false;
            BoardVm = new BoardViewModel(vm.BoardWidth, vm.BoardHeight, vm.RectCount);
            try {
                var channel = GrpcChannel.ForAddress("https://localhost:7005");
                var client = new RectMoving.RectMovingClient(channel);
                var request = new ConfigRequest();
                request.TasksCount = vm.TaskCount;
                request.Board = new Board() { Height = vm.BoardHeight, Width = vm.BoardWidth, RectsCount = vm.RectCount };

                using var call = client.SetConfig(request);
                while (await call.ResponseStream.MoveNext(CancellationToken.None)) {
                    BoardVm.SetRectCoords(call.ResponseStream.Current);
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
