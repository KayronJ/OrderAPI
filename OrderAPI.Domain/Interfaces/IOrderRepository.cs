using OrderAPI.Domain.Entities;
using OrderAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task<List<Order>> GetAllAsync();
        Task AddOrderAsync(Order order);
        Task AddOccurrenceAsync(Order order);
        Task RemoveOccurenceAsync(Order order);
        Task UpdateAsync(Order order);
    }
}
