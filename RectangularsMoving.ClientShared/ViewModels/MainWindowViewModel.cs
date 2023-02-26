using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
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
        private readonly RectMoving.RectMovingClient _client;
        private Metadata _headers;
        
        [ObservableProperty] private bool _isConnectionNeeds;
        [ObservableProperty] private SettingsViewModel _settingsVm;
        [ObservableProperty] private BoardViewModel _boardVm;

        public MainWindowViewModel(BoardViewModel boardVm, RectMoving.RectMovingClient client) {
            _client = client;
            _headers = new Metadata();
            _headers.Add("ClientId", Guid.NewGuid().ToString());

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
                var request = new ConfigRequest();
                request.TasksCount = vm.TaskCount;
                request.TaskDelay = vm.TaskDelay;
                request.MaxMovingDistance = vm.MaxMovingDistance;
                request.Board = new Board() {
                    Height = vm.BoardHeight, Width = vm.BoardWidth, RectsCount = vm.RectCount
                };
                _isReceivingAllowed = true;
                using var call = _client.StartAsync(request, _headers);
                while (_isReceivingAllowed && await call.ResponseStream.MoveNext(CancellationToken.None)) {
                    Task.Run(() => {
                        if (_timer.Enabled)
                            AddRect(call.ResponseStream.Current);
                        else BoardVm.SetRectCoords(call.ResponseStream.Current);
                    });
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }
        [RelayCommand]
        private void Stop() {
            try{
                _client.Stop(new Empty(), _headers);
            }
            catch(Exception e){
                    Console.WriteLine(e);
            }
            finally{
                _isReceivingAllowed = false;
                BoardVm.Rects.Clear();
                IsConnectionNeeds = true;
            }
        }
    }
}
