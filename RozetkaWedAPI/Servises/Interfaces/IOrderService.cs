using RozetkaWedAPI.Models;

namespace RozetkaWedAPI.Servises.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<Order> AddAsync(Order order);
        Task<Order?> UpdateAsync(int id, Order updatedOrder);
        Task<bool> DeleteAsync(int id);
    }
}