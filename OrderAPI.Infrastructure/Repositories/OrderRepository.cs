using Microsoft.EntityFrameworkCore;
using OrderAPI.Domain.Entities;
using OrderAPI.Domain.Enums;
using OrderAPI.Domain.Interfaces;
using OrderAPI.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appContext;
        public OrderRepository(AppDbContext context)
        {
            _appContext = context;
        }
        public async Task AddAsync(Order order)
        {
            await _appContext.Orders.AddAsync(order);
            await _appContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _appContext.Orders
                .Include(o => o.Occurrences)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order is null) 
                return;

            _appContext.Orders.Remove(order);
            await _appContext.SaveChangesAsync();
        }

        public Task<List<Order>> GetAllAsync()
        {
            return _appContext.Orders
                    .Include(o => o.Occurrences)
                    .ToListAsync();
        }

        public Task<Order> GetByIdAsync(int id)
        {
            return _appContext.Orders
                .Include(o => o.Occurrences)
                .FirstOrDefaultAsync(o => o.OrderId == id);    
        }

        public async Task UpdateAsync(Order order)
        {
            _appContext.Orders.Update(order);
            await _appContext.SaveChangesAsync();
        }
    }
}
