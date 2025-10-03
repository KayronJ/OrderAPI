using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Application.DTOs.Requests
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Username é obrigatório")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password é obrigatório")]
        public string Password { get; set; } = string.Empty;
    }
}
