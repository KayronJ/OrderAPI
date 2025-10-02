using OrderAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Application.DTOs.Requests
{
    public class OccurrenceResponseDto
    {
        public int OccurrenceId { get; set; }
        public EOccurrenceType OccurrenceType { get; set; }
        public DateTime OccurrenceTime { get; set; }
        public bool FinisherInd { get; set; }
    }
}
