using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Application.DTOs.Requests
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderTime { get; set; }
        public bool DeliveredInd { get; set; }
        public List<OccurrenceResponseDto> Occurrences { get; set; }
    }
}
