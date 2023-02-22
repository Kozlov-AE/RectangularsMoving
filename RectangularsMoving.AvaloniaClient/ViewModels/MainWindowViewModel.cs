﻿using CommunityToolkit.Mvvm.ComponentModel;
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
        }

        [ObservableProperty] private bool _isConnectionNeeds;
        [ObservableProperty] private SettingsViewModel _settingsVm;

        [RelayCommand]
        private async Task Connect(SettingsViewModel vm) {
            IsConnectionNeeds = false;
            try {
                var channel = GrpcChannel.ForAddress("https://localhost:7005");
                var client = new RectMoving.RectMovingClient(channel);
                var request = new ConfigRequest();
                request.TasksCount = 50;
                request.Board = new Board() { Height = 500, Width = 500, RectsCount = 100 };

                using var call = client.SetConfig(request);
                while (await call.ResponseStream.MoveNext(CancellationToken.None)) {
                    Console.WriteLine($"Rect: {call.ResponseStream.Current.Id}: {call.ResponseStream.Current.X},{call.ResponseStream.Current.Y}");
                    Debug.WriteLine($">>>Rect: {call.ResponseStream.Current.Id}: {call.ResponseStream.Current.X},{call.ResponseStream.Current.Y}");
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
        [RelayCommand]
        private void Stop() {
            
        }
    }
}
