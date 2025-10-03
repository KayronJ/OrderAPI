// OrderAPI.Tests/Domain/OrderDomainTests.cs
using System;
using System.IO;
using System.Linq;
using OrderAPI.Domain.Entities;
using OrderAPI.Domain.Enums;
using Xunit;

namespace OrderAPI.Tests.Domain
{
    public class OrderDomainTests
    {
        [Fact]
        public void CriarPedido_DeveInicializarCorretamente()
        {
            var now = DateTime.Now;
            var order = new Order(12345, now);

            Assert.Equal(12345, order.OrderNumber);
            Assert.Equal(now, order.OrderTime);
            Assert.False(order.DeliveredInd);
            Assert.Empty(order.Occurrences);
        }

        [Fact]
        public void AddNewOccurrence_PrimeiraOcorrencia_DeveAdicionar()
        {
            var order = new Order(12345, DateTime.Now);

            order.AddNewOccurrence(EOccurrenceType.OnDeliveryRoute, DateTime.Now);

            Assert.Single(order.Occurrences);
            Assert.Equal(EOccurrenceType.OnDeliveryRoute, order.Occurrences[0].OccurrenceType);
            Assert.False(order.DeliveredInd);
        }

        [Fact]
        public void AddNewOccurrence_SegundaOcorrencia_DeveSerFinalizadoraQuandoSuccessfullyDelivered()
        {
            var order = new Order(12345, DateTime.Now);

            order.AddNewOccurrence(EOccurrenceType.OnDeliveryRoute, DateTime.Now.AddMinutes(-30));
            order.AddNewOccurrence(EOccurrenceType.SuccessfullyDelivered, DateTime.Now);

            Assert.Equal(2, order.Occurrences.Count);
            Assert.Equal(EOccurrenceType.SuccessfullyDelivered, order.Occurrences.Last().OccurrenceType);
            Assert.True(order.DeliveredInd);
        }

        [Fact]
        public void AddNewOccurrence_DuplicadaEm10Minutos_DeveLancarInvalidOperationException()
        {
            var order = new Order(12345, DateTime.Now);
            var now = DateTime.Now;

            order.AddNewOccurrence(EOccurrenceType.OnDeliveryRoute, now.AddMinutes(-5));

            Assert.Throws<InvalidOperationException>(() =>
                order.AddNewOccurrence(EOccurrenceType.OnDeliveryRoute, now));
        }

        [Fact]
        public void DeleteOccurrence_DeveRemoverOcorrenciaComSucesso()
        {
            var order = new Order(12345, DateTime.Now);

            order.AddNewOccurrence(EOccurrenceType.OnDeliveryRoute, DateTime.Now);
            var occId = order.Occurrences[0].OccurrenceId;

            order.DeleteOccurrence(occId);

            Assert.Empty(order.Occurrences);
        }

        [Fact]
        public void DeleteOccurrence_EmPedidoFinalizado_DeveLancarInvalidDataException()
        {
            var order = new Order(12345, DateTime.Now);

            order.AddNewOccurrence(EOccurrenceType.OnDeliveryRoute, DateTime.Now.AddMinutes(-30));
            order.AddNewOccurrence(EOccurrenceType.SuccessfullyDelivered, DateTime.Now);

            var occurrenceId = order.Occurrences.First().OccurrenceId;

            Assert.Throws<InvalidDataException>(() => order.DeleteOccurrence(occurrenceId));
        }
    }
}
