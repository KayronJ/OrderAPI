using OrderAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Application.DTOs.Requests
{
    public class AddOccurrenceRequestDto
    {
        [Required(ErrorMessage = "O tipo de ocorrência é obrigatório")]
        [EnumDataType(typeof(EOccurrenceType), ErrorMessage = "Tipo de ocorrência inválido")]
        public EOccurrenceType OccurrenceType { get; set; }

        [Required(ErrorMessage = "A hora da ocorrência é obrigatória")]
        public DateTime OccurrenceTime { get; set; }
    }
}
