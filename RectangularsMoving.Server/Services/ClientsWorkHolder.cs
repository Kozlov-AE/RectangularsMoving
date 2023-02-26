using System.Collections.Concurrent;

namespace RectangularsMoving.Server.Services {
    public class ClientsWorkHolder {
        private readonly ConcurrentDictionary<string, bool> _clientWorks = new ();
        public bool CheckIfClientIsWorking(string id) {
            return _clientWorks.GetValueOrDefault(id, false);
        }

        public void ClientStart(string id) {
            if (string.IsNullOrEmpty(id)) throw new Exception("Client can't has null or empty Id!");
            _clientWorks.TryAdd(id, true);
        }

        public void ClientStop(string? id) {
            if (string.IsNullOrEmpty(id)) return;
            if (!_clientWorks.TryRemove(id, out _)) {
                _clientWorks.AddOrUpdate(id, false, (s, b) => false);
            }
            
        }
    }
}