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
        internal Occurrence(EOccurrenceType type, DateTime ocurrenceTime, bool isFinisher)
        {
            OccurrenceType = type;
            OccurrenceTime = ocurrenceTime;
            FinisherInd = isFinisher;
        }

        public int OccurrenceId { get; private set; }
        public EOccurrenceType OccurrenceType { get; private set; }
        public DateTime OccurrenceTime { get; private set; }
        public bool FinisherInd { get; private set; }
    }
}
