using OrderAPI.Application.DTOs.Requests;
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
        Task CreateOrderAsync(CreateOrderRequestDto orderRequest);
        Task<OrderResponseDto> GetOrderByIdAsync(int id);
        Task<List<OrderResponseDto>> GetAllOrdersAsync();
        Task AddOccurrenceAsync(int orderId, AddOccurrenceRequestDto request);
        Task DeleteOccurrenceAsync(int orderId, int occurrenceId);
    }
}