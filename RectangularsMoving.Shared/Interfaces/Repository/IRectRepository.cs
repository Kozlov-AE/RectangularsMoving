using RectangularsMoving.Shared.Interfaces.Repository.Models;
using RectangularsMoving.Shared.Models;

namespace RectangularsMoving.Shared.Interfaces.Repository {
    public interface IRectRepository {
        /// <summary>
        /// Add or update item in the repository
        /// </summary>
        /// <param name="rect">New item</param>
        /// <returns>Null if it have a error during adding or updating</returns>
        Task<IRect?> AddOrUpdateAsync(IRect rect);
        /// <summary>
        /// Get item by Id
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>Null if item not found</returns>
        Task<IRect?> GetAsync(string id);
        /// <summary>
        /// Returns all values from storage
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IRect>> GetAllAsync();
        /// <summary>
        /// Remove item by id
        /// </summary>
        /// <param name="id">Item's id</param>
        /// <returns>Removed item or null if fault</returns>
        Task<IRect?> DeleteAsync(string id);
    }
}