using OrderAPI.Application.DTOs.Requests;
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
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            var returnData = orders.Select(order => new OrderResponseDto
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                OrderTime = order.OrderTime,
                DeliveredInd = order.DeliveredInd,
                Occurrences = order.Occurrences.Select(o => new OccurrenceResponseDto
                {
                    OccurrenceId = o.OccurrenceId,
                    OccurrenceType = o.OccurrenceType,
                    OccurrenceTime = o.OccurrenceTime,
                    FinisherInd = o.FinisherInd
                }).ToList()
            }).ToList();

            return returnData;
        }

        public async Task<OrderResponseDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
                throw new KeyNotFoundException($"Pedido com ID {orderId} não encontrado");

            var returnData = new OrderResponseDto
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                OrderTime = order.OrderTime,
                DeliveredInd = order.DeliveredInd,
                Occurrences = order.Occurrences.Select(o => new OccurrenceResponseDto
                {
                    OccurrenceId = o.OccurrenceId,
                    OccurrenceType = o.OccurrenceType,
                    OccurrenceTime = o.OccurrenceTime,
                    FinisherInd = o.FinisherInd
                }).ToList()
            };

            return returnData;
        }

        public async Task CreateOrderAsync(CreateOrderRequestDto orderRequest)
        {
            var order = new Order(orderRequest.OrderNumber, orderRequest.OrderTime);
            await _orderRepository.AddAsync(order);
        }

        public async Task AddOccurrenceAsync(int orderId, AddOccurrenceRequestDto request)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
                throw new KeyNotFoundException($"Pedido com ID {orderId} não encontrado");

            order.AddNewOccurrence(request.OccurrenceType, request.OccurrenceTime);

            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOccurrenceAsync(int orderId, int occurrenceId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
                throw new KeyNotFoundException($"Pedido com ID {orderId} não encontrado");

            order.DeleteOccurrence(occurrenceId);

            await _orderRepository.UpdateAsync(order);
        }
    }
}
