using RectangularsMoving.Shared.Interfaces.Repository;
using RectangularsMoving.Shared.Interfaces.Repository.Models;
using RectangularsMoving.Shared.Models;
using System.Collections.Concurrent;
using System.Reflection.Metadata.Ecma335;

namespace InMemoryRepository;

public class RectRepository : IRectRepository {
    private readonly ConcurrentDictionary<string, IRect> _repo = new ();
    private readonly object _lock = new ();
    public Task<IRect?> AddOrUpdateAsync(IRect rect) {
        try {
            if (_repo.TryGetValue(rect.Id, out var old)) {
                lock (_lock) {
                    return Task.FromResult<IRect?>(_repo.AddOrUpdate(rect.Id, rect, (s, rect1) => rect1));
                }
            }

            if (string.IsNullOrEmpty(rect.Id)) {
                RectModel newRect = (RectModel)rect with { Id = Guid.NewGuid().ToString() };
                if (_repo.TryAdd(newRect.Id, newRect)) {
                    return Task.FromResult<IRect?>(newRect);
                }
            }

            return Task.FromResult<IRect?>(null);
        }
        catch (Exception ex) {
            return Task.FromResult<IRect?>(null);
        }
    }

    public Task<IRect?> GetAsync(string id) {
        if (string.IsNullOrEmpty(id)) return Task.FromResult<IRect?>(null); 
        _repo.TryGetValue(id, out var result);
        return Task.FromResult<IRect?>(result); 
    }

    public Task<IEnumerable<IRect>> GetAllAsync() {
        return Task.FromResult<IEnumerable<IRect>>(_repo.Values);
    }

    public Task<IRect?> DeleteAsync(string id) {
        _repo.Remove(id, out var result);
        return Task.FromResult<IRect?>(result);
    }
}