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
        Task<Order> GetAllAsync(int id);
        Task AddOrderAsync(Order order);
        Task AddOccurrenceAsync(Order order);
        Task UpdateAsync(Order order);
    }
}
