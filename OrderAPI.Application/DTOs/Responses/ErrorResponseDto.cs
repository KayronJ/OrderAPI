using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Application.DTOs.Responses
{
    /// <summary>
    /// Resposta de erro padrão da API
    /// </summary>
    public class ErrorResponseDto
    {
        /// <summary>
        /// Mensagem de erro
        /// </summary>
        /// <example>Pedido não encontrado</example>
        public string Error { get; set; }

        /// <summary>
        /// Timestamp do erro
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public ErrorResponseDto(string error)
        {
            Error = error;
        }
    }

}
