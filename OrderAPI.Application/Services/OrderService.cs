using OrderAPI.Application.DTOs;
using OrderAPI.Application.Interfaces;
using OrderAPI.Domain.Entities;
using OrderAPI.Domain.Enums;
using OrderAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _order;

        public OrderService(IOrderRepository orderRepository)
        {
            _order = orderRepository;
        }
        
        public async Task AddOccurrence(int orderId, EOccurrenceType type, DateTime dateTime)
        {
            var order = await _order.GetByIdAsync(orderId);
            order.AddNewOccurrence(type, dateTime);

            await _order.AddOccurrenceAsync(order);
        }

        public async Task CreateOrderAsync(OrderRequestDto orderRequest)
        {
            var order = new Order(orderRequest.OrderNumber, orderRequest.OrderTime);
            await _order.AddOrderAsync(order);
        }

        public Task DeleteOccurrence(int occurrenceId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _order.GetByIdAsync(orderId);
            return order;
        }
    }
}
