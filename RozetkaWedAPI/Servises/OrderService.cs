using Microsoft.EntityFrameworkCore;
using RozetkaWedAPI.Data;
using RozetkaWedAPI.Servises.Interfaces;
using RozetkaWedAPI.Models;

namespace RozetkaWebAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly StoreContext _context;
        public OrderService(StoreContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(o => o.OrderItems).ToListAsync();
        }
        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<Order> AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<Order?> UpdateAsync(int id, Order updatedOrder)
        {
            var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return null;
            order.BuyerName = updatedOrder.BuyerName;
            order.BuyerEmail = updatedOrder.BuyerEmail;
            order.OrderDate = updatedOrder.OrderDate;
            order.OrderItems = updatedOrder.OrderItems;
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}