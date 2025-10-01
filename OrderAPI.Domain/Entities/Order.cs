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


        public Order(int orderNumber, bool deliveredInd = false)
        {
            OrderNumber = orderNumber;
            OrderTime = DateTime.UtcNow;
            DeliveredInd = deliveredInd;
        }

        public void AddNewOccurrence(EOccurrenceType type, DateTime newOccurrenceTime)
        {
            if (DeliveredInd)
                throw new InvalidDataException("Não é possivel adicionar Ocorrencias à Pedidos finalizados.");

            var occurrenceAlreadyExists = Occurrences?.LastOrDefault(o => o.OccurrenceType == type);

            if (occurrenceAlreadyExists != null
                && (newOccurrenceTime - occurrenceAlreadyExists.OccurrenceTime) < TimeSpan.FromMinutes(10))
                throw new InvalidDataException("É possivel adicionar uma ocorrencia do mesmo tipo somente depois de um periodo de 10 minutos da anterior.");

            var isFinisherOccurrence = Occurrences.Count == 1;

            Occurrence occurrence = new Occurrence(type, newOccurrenceTime, isFinisherOccurrence);

            Occurrences.Add(occurrence);

            var orderWasDelivered = type == EOccurrenceType.SuccessfullyDelivered ? true : false;
            if (isFinisherOccurrence)
            {
                DeliveredInd = orderWasDelivered;
            }
        }

        public void DeleteOccurrence(int occurrenceId)
        {
            if (DeliveredInd)
                throw new InvalidDataException("Não é possivel deletar Ocorrencias à Pedidos finalizados.");

            Occurrences?.RemoveAll(o => o.OccurrenceId == occurrenceId);
        }

    }
}
