using OrderAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Application.DTOs.Requests
{
    /// <summary>
    /// DTO para adicionar uma ocorrência a um pedido
    /// </summary>
    public class AddOccurrenceRequestDto
    {
        /// <summary>
        /// Tipo da ocorrência
        /// </summary>
        /// <example>OnDeliveryRoute</example>
        [Required(ErrorMessage = "O tipo de ocorrência é obrigatório")]
        [EnumDataType(typeof(EOccurrenceType), ErrorMessage = "Tipo de ocorrência inválido")]
        public EOccurrenceType OccurrenceType { get; set; }

        /// <summary>
        /// Data e hora em que a ocorrência aconteceu
        /// </summary>
        /// <example>2025-10-03T15:00:00</example>
        [Required(ErrorMessage = "A hora da ocorrência é obrigatória")]
        public DateTime OccurrenceTime { get; set; }
    }
}
