using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Application.DTOs.Requests;
using OrderAPI.Application.DTOs.Responses;
using OrderAPI.Application.Interfaces;

namespace OrderAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Realiza login na API
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST /api/auth/login
        ///     {
        ///        "username": "admin",
        ///        "password": "admin123"
        ///     }
        /// 
        /// **Importante:**
        /// - O token retornado deve ser incluído no header Authorization de todas as requisições
        /// - Formato: `Authorization: Bearer {token}`
        /// - Token válido por 2 horas
        /// </remarks>
        /// <param name="request">Credenciais de login</param>
        /// <response code="200">Login realizado com sucesso</response>
        /// <response code="401">Credenciais inválidas</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Tentativa de login inválida para usuário: {Username}", request.Username);
                return Unauthorized(new ErrorResponseDto(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar login");
                return StatusCode(500, new ErrorResponseDto("Erro interno do servidor"));
            }
        }

        /// <summary>
        /// Registra um novo usuário
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST /api/auth/register
        ///     {
        ///        "username": "novouser",
        ///        "password": "senha123",
        ///        "role": "User"
        ///     }
        /// 
        /// **Roles Disponíveis:**
        /// - `User` - Usuário comum (padrão)
        /// - `Admin` - Administrador
        /// 
        /// **Validações:**
        /// - Username deve ter no mínimo 3 caracteres
        /// - Password deve ter no mínimo 6 caracteres
        /// - Username deve ser único
        /// </remarks>
        /// <param name="request">Dados do novo usuário</param>
        /// <response code="201">Usuário criado com sucesso</response>
        /// <response code="400">Dados inválidos ou username já existe</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _authService.RegisterAsync(request);
                return StatusCode(201, response);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Tentativa de registro com username duplicado: {Username}", request.Username);
                return BadRequest(new ErrorResponseDto(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar registro");
                return StatusCode(500, new ErrorResponseDto("Erro interno do servidor"));
            }
        }

        /// <summary>
        /// Valida se o token JWT ainda é válido
        /// </summary>
        /// <remarks>
        /// Endpoint protegido que requer autenticação.
        /// Utilize para verificar se o token do usuário ainda está válido.
        /// 
        /// **Header necessário:**
        /// - `Authorization: Bearer {seu-token}`
        /// </remarks>
        /// <response code="200">Token válido</response>
        /// <response code="401">Token inválido ou expirado</response>
        [HttpGet("validate")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public IActionResult ValidateToken()
        {
            var username = User.Identity?.Name;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new
            {
                message = "Token válido",
                username = username,
                role = role
            });
        }
    }
}