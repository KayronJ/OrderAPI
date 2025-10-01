using OrderAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Application.DTOs
{
    public class OrderRequestDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "O valor deve ser um número positivo.")]
        public int OrderNumber { get; private set; }
        public List<Occurrence> Occurrences { get; private set; } = new List<Occurrence>();
        public DateTime OrderTime { get; private set; }
        public bool DeliveredInd { get; private set; }
    }
}
