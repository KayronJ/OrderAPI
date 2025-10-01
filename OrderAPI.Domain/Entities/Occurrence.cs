using OrderAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Domain.Entities
{
    public class Occurrence
    {
        public int OccurrenceId { get; private set; }
        public EOccurrenceType OccurrenceType { get; private set; }
        public DateTime OccurrenceTime { get; private set; }
        public bool FinisherInd { get; private set; }

        public Occurrence(EOccurrenceType type, bool isFinisher = false )
        {
            OccurrenceType = type;
            FinisherInd = isFinisher;
            OccurrenceTime = DateTime.UtcNow;
        }

        public void IsFinisherOccurrence(bool secoundOccurence = false)
        {
            FinisherInd = secoundOccurence;
        }

        public bool IsDelivererOccurrence(EOccurrenceType type)
        {

            return type == EOccurrenceType.SuccessfullyDelivered ? true : false;
        }
    }
}
