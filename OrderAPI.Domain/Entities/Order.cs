using OrderAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Domain.Entities
{

  
    public class Order
    {

        public int OrderId { get; private set; }
        public int OrderNumber { get; private set; }
        public List<Occurrence> Occurrences { get;  private set; } = new List<Occurrence>();
        public DateTime OrderTime { get; private set; }
        public bool DeliveredInd { get; private set; }


        public Order(int orderNumber, DateTime orderTime)
        {
            OrderNumber = orderNumber;
            OrderTime = orderTime;
            DeliveredInd = false;
        }

        public void AddNewOccurrence(EOccurrenceType type, DateTime newOccurrenceTime)
        {
            if (DeliveredInd)
                throw new InvalidDataException("Não é possivel adicionar Ocorrencias à Pedidos finalizados.");

            ValidateDuplicateOccurrence(type, newOccurrenceTime);

            var isFinisherOccurrence = Occurrences.Count == 1;
                
            Occurrence occurrence = new Occurrence(type, newOccurrenceTime, isFinisherOccurrence);
            Occurrences.Add(occurrence);

            if (isFinisherOccurrence)
            {
                DeliveredInd = type == EOccurrenceType.SuccessfullyDelivered;
            }
        }

        public void DeleteOccurrence(int occurrenceId)
        {
            if (DeliveredInd)
                throw new InvalidDataException("Não é possivel deletar Ocorrencias à Pedidos finalizados.");

            var occurrence = Occurrences.FirstOrDefault(o => o.OccurrenceId == occurrenceId);

            if (occurrence == null)
                throw new InvalidOperationException("Ocorrência não encontrada.");

            Occurrences.Remove(occurrence);
        }

        private void ValidateDuplicateOccurrence(EOccurrenceType type, DateTime newOccurrenceTime)
        {
            var recentOccurrences = Occurrences
              .Where(o => o.OccurrenceType == type)
              .Where(o => (newOccurrenceTime - o.OccurrenceTime) < TimeSpan.FromMinutes(10))
              .ToList();

            if (recentOccurrences.Any())
            {
                throw new InvalidOperationException(
                    "Não é possível adicionar uma ocorrência do mesmo tipo em menos de 10 minutos da anterior.");
            }
        }

    }
}
