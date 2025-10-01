using OrderAPI.Application.DTOs;
using OrderAPI.Domain.Entities;
using OrderAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Application.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(OrderRequestDto orderRequest);
        Task<Order> GetOrderByIdAsync(int id);
        Task<List<Order>> GetAllOrdersAsync();
        Task AddOccurrence(int orderId, EOccurrenceType type, DateTime dateTime);
        Task DeleteOccurrence(int occurrenceId);
    }
}
