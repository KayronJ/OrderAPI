// OrderAPI.Tests/Services/OrderServiceTests.cs
using Microsoft.EntityFrameworkCore;
using Moq;
using OrderAPI.Application.DTOs.Requests;
using OrderAPI.Application.Services;
using OrderAPI.Domain.Entities;
using OrderAPI.Domain.Enums;
using OrderAPI.Infrastructure.Persistence;
using OrderAPI.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OrderAPI.Tests.Services
{
    public class OrderServiceTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateOrderAsync_DeveCriarPedidoNoInMemory()
        {
            using var ctx = CreateContext();
            var repo = new OrderRepository(ctx);
            var service = new OrderService(repo);

            var dto = new CreateOrderRequestDto { OrderNumber = 12345, OrderTime = DateTime.Now };

            await service.CreateOrderAsync(dto);

            var list = await ctx.Orders.ToListAsync();
            Assert.Single(list);
            Assert.Equal(12345, list[0].OrderNumber);
        }

        [Fact]
        public async Task GetOrderByIdAsync_DeveRetornarPedidoExistente()
        {
            using var ctx = CreateContext();
            var repo = new OrderRepository(ctx);
            var service = new OrderService(repo);

            var order = new Order(22222, DateTime.Now);
            await repo.AddAsync(order);

            var result = await service.GetOrderByIdAsync(order.OrderId);

            Assert.NotNull(result);
            Assert.Equal(22222, result.OrderNumber);
        }

        [Fact]
        public async Task GetOrderByIdAsync_DeveLancarQuandoNaoExistir()
        {
            using var ctx = CreateContext();
            var repo = new OrderRepository(ctx);
            var service = new OrderService(repo);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetOrderByIdAsync(999999));
        }

        [Fact]
        public async Task AddOccurrenceAsync_PrimeiraOcorrencia_Persiste()
        {
            using var ctx = CreateContext();
            var repo = new OrderRepository(ctx);
            var service = new OrderService(repo);

            var order = new Order(33333, DateTime.Now);
            await repo.AddAsync(order);

            var dto = new AddOccurrenceRequestDto { OccurrenceType = EOccurrenceType.OnDeliveryRoute, OccurrenceTime = DateTime.Now };

            await service.AddOccurrenceAsync(order.OrderId, dto);

            var loaded = await ctx.Orders.Include(o => o.Occurrences).FirstAsync(o => o.OrderId == order.OrderId);
            Assert.Single(loaded.Occurrences);
            Assert.False(loaded.Occurrences[0].FinisherInd);
        }

        [Fact]
        public async Task AddOccurrenceAsync_SegundaOcorrencia_MarcaFinalizadora()
        {
            using var ctx = CreateContext();
            var repo = new OrderRepository(ctx);
            var service = new OrderService(repo);

            var order = new Order(44444, DateTime.Now);
            await repo.AddAsync(order);

            await service.AddOccurrenceAsync(order.OrderId, new AddOccurrenceRequestDto
            {
                OccurrenceType = EOccurrenceType.OnDeliveryRoute,
                OccurrenceTime = DateTime.Now.AddHours(-1)
            });

            await service.AddOccurrenceAsync(order.OrderId, new AddOccurrenceRequestDto
            {
                OccurrenceType = EOccurrenceType.CustomerAbsent,
                OccurrenceTime = DateTime.Now
            });

            var loaded = await ctx.Orders.Include(o => o.Occurrences).FirstAsync(o => o.OrderId == order.OrderId);
            Assert.Equal(2, loaded.Occurrences.Count);
            Assert.True(loaded.Occurrences[1].FinisherInd);
        }

        [Fact]
        public async Task AddOccurrenceAsync_DeveImpedirDuplicadaEm10Minutos()
        {
            using var ctx = CreateContext();
            var repo = new OrderRepository(ctx);
            var service = new OrderService(repo);

            var now = DateTime.Now;
            var order = new Order(55555, now);
            await repo.AddAsync(order);

            await service.AddOccurrenceAsync(order.OrderId, new AddOccurrenceRequestDto
            {
                OccurrenceType = EOccurrenceType.OnDeliveryRoute,
                OccurrenceTime = now.AddMinutes(-5)
            });

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.AddOccurrenceAsync(order.OrderId, new AddOccurrenceRequestDto
                {
                    OccurrenceType = EOccurrenceType.OnDeliveryRoute,
                    OccurrenceTime = now
                }));
        }

        [Fact]
        public async Task AddOccurrenceAsync_SuccessfullyDelivered_MarcaDeliveredInd()
        {
            using var ctx = CreateContext();
            var repo = new OrderRepository(ctx);
            var service = new OrderService(repo);

            var order = new Order(66666, DateTime.Now);
            await repo.AddAsync(order);

            await service.AddOccurrenceAsync(order.OrderId, new AddOccurrenceRequestDto
            {
                OccurrenceType = EOccurrenceType.OnDeliveryRoute,
                OccurrenceTime = DateTime.Now.AddMinutes(-30)
            });

            await service.AddOccurrenceAsync(order.OrderId, new AddOccurrenceRequestDto
            {
                OccurrenceType = EOccurrenceType.SuccessfullyDelivered,
                OccurrenceTime = DateTime.Now
            });

            var loaded = await ctx.Orders.FirstAsync(o => o.OrderId == order.OrderId);
            Assert.True(loaded.DeliveredInd);
        }

        [Fact]
        public async Task DeleteOccurrenceAsync_DeveExcluirComSucesso()
        {
            using var ctx = CreateContext();
            var repo = new OrderRepository(ctx);
            var service = new OrderService(repo);

            var order = new Order(77777, DateTime.Now);
            await repo.AddAsync(order);

            await service.AddOccurrenceAsync(order.OrderId, new AddOccurrenceRequestDto
            {
                OccurrenceType = EOccurrenceType.OnDeliveryRoute,
                OccurrenceTime = DateTime.Now
            });

            var loaded = await ctx.Orders.Include(o => o.Occurrences).FirstAsync(o => o.OrderId == order.OrderId);
            var occId = loaded.Occurrences[0].OccurrenceId;

            await service.DeleteOccurrenceAsync(order.OrderId, occId);

            var after = await ctx.Orders.Include(o => o.Occurrences).FirstAsync(o => o.OrderId == order.OrderId);
            Assert.Empty(after.Occurrences);
        }

        [Fact]
        public async Task DeleteOccurrenceAsync_DeveImpedirExclusaoEmPedidoConcluido()
        {
            using var ctx = CreateContext();
            var repo = new OrderRepository(ctx);
            var service = new OrderService(repo);

            var order = new Order(88888, DateTime.Now);
            await repo.AddAsync(order);

            await service.AddOccurrenceAsync(order.OrderId, new AddOccurrenceRequestDto
            {
                OccurrenceType = EOccurrenceType.OnDeliveryRoute,
                OccurrenceTime = DateTime.Now.AddMinutes(-30)
            });

            await service.AddOccurrenceAsync(order.OrderId, new AddOccurrenceRequestDto
            {
                OccurrenceType = EOccurrenceType.SuccessfullyDelivered,
                OccurrenceTime = DateTime.Now
            });

            var loaded = await ctx.Orders.Include(o => o.Occurrences).FirstAsync(o => o.OrderId == order.OrderId);
            var occId = loaded.Occurrences[0].OccurrenceId;

            await Assert.ThrowsAsync<InvalidDataException>(() => service.DeleteOccurrenceAsync(order.OrderId, occId));
        }
    }
}
