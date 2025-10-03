using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Application.DTOs.Responses
{
    public class ErrorResponseDto
    {
        public string Error { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public ErrorResponseDto(string error)
        {
            Error = error;
        }
    }

}
