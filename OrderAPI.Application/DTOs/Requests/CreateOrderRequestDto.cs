using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Application.DTOs.Requests
{
    public class CreateOrderRequestDto
    {
        [Required(ErrorMessage = "O número do pedido é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O número do pedido deve ser maior que zero")]
        public int OrderNumber { get; set; }
        [Required(ErrorMessage = "A hora do pedido é obrigatória")]
        public DateTime OrderTime { get; set; }
    }
}
