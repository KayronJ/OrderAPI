using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Application.DTOs.Requests
{
    /// <summary>
    /// DTO para criação de um novo pedido
    /// </summary>
    public class CreateOrderRequestDto
    {
        /// <summary>
        /// Número identificador único do pedido
        /// </summary>
        /// <example>12345</example>
        [Required(ErrorMessage = "O número do pedido é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O número do pedido deve ser maior que zero")]
        public int OrderNumber { get; set; }

        /// <summary>
        /// Data e hora em que o pedido foi realizado
        /// </summary>
        /// <example>2025-10-03T14:30:00</example>
        [Required(ErrorMessage = "A hora do pedido é obrigatória")]
        public DateTime OrderTime { get; set; }
    }
}
