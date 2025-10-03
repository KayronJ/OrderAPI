using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Application.DTOs.Requests
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Username é obrigatório")]
        [MinLength(3, ErrorMessage = "Username deve ter no mínimo 3 caracteres")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password é obrigatório")]
        [MinLength(6, ErrorMessage = "Password deve ter no mínimo 6 caracteres")]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "User";
    }
}
