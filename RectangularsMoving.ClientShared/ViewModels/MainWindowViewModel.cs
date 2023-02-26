using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grpc.Net.Client;
using System.Collections.Concurrent;
using System.Timers;
using Timer = System.Timers.Timer;
using RectangularsMoving.Protos;

namespace RectangularsMoving.ClientShared.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject {
        private ConcurrentDictionary<int, Rect> _incomingRects;
        private object _incomingRectsLock = new object();
        private System.Timers.Timer _timer;
        private bool _isReceivingAllowed = false;
        
        [ObservableProperty] private bool _isConnectionNeeds;
        [ObservableProperty] private SettingsViewModel _settingsVm;
        [ObservableProperty] private BoardViewModel _boardVm;

        public MainWindowViewModel(BoardViewModel boardVm) {
            _boardVm = boardVm;
            IsConnectionNeeds = true;
            SettingsVm = new SettingsViewModel();

            _incomingRects = new ConcurrentDictionary<int, Rect>();

            _timer = new ();
            _timer.Elapsed += (sender, args) => SendAllRectsToUi();
        }

        private void AddRect(Rect rect) {
            if (rect.IsReflectioning) {
                if (_incomingRects.TryRemove(rect.Id, out _)) {
                    _boardVm.SetRectCoords(rect);
                }
            }
            else {
                lock (_incomingRectsLock) {
                    _incomingRects.AddOrUpdate(rect.Id, rect, (i, rect1) => rect1);
                }
            }
        }

        private void SendAllRectsToUi() {
            if (_incomingRects.Count == 0) return;
            Parallel.ForEach(_incomingRects, r => {
                _boardVm.SetRectCoords(r.Value);
            });
            _incomingRects.Clear();
        }

        [RelayCommand]
        private async Task Connect(SettingsViewModel vm) {
            IsConnectionNeeds = false;
            if (vm.LocalBufferInterval > 0) {
                _timer.Interval = vm.LocalBufferInterval;
                _timer.Start();
            }
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
                _isReceivingAllowed = true;
                using var call = client.SetConfig(request);
                while (_isReceivingAllowed && await call.ResponseStream.MoveNext(CancellationToken.None)) {
                    Task.Run(() => {
                        if (_timer.Enabled)
                            AddRect(call.ResponseStream.Current);
                        else BoardVm.SetRectCoords(call.ResponseStream.Current);
                    });
                }
                channel.Dispose();
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }
        
        [RelayCommand]
        private void Stop() {
            _isReceivingAllowed = false;
            IsConnectionNeeds = true;
        }
    }
}
