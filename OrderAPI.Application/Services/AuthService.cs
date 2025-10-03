
// Application/Services/AuthService.cs
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderAPI.Application.DTOs.Requests;
using OrderAPI.Application.DTOs.Responses;
using OrderAPI.Application.Interfaces;
using OrderAPI.Domain.Entities;
using OrderAPI.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderAPI.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Credenciais inválidas");

            var token = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddHours(2);

            return new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                ExpiresAt = expiresAt
            };
        }

        public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            if (!(await _userRepository.ExistsByUsernameAsync(request.Username)))
                throw new InvalidOperationException("Username já existe");

            var user = new User
            {
                Username = request.Username,
                PasswordHash = HashPassword(request.Password),
                Role = request.Role,
                CreatedAt = DateTime.Now
            };

            await _userRepository.AddAsync(user);

            var token = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddHours(2);

            return new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                ExpiresAt = expiresAt
            };
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key não configurada");
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? "OrderAPI";
            var jwtAudience = _configuration["Jwt:Audience"] ?? "OrderAPI";

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}