using Microsoft.AspNetCore.Mvc;
using OrderAPI.Application.DTOs.Requests;
using OrderAPI.Application.DTOs.Responses;
using OrderAPI.Application.Interfaces;

namespace OrderAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly IOrderService _service;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService service, ILogger<OrdersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Cadastra um novo pedido
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST /api/orders
        ///     {
        ///        "orderNumber": 12345,
        ///        "orderTime": "2025-10-03T14:30:00"
        ///     }
        /// 
        /// **Validações:**
        /// - OrderNumber deve ser maior que zero
        /// - OrderTime é obrigatório
        /// </remarks>
        /// <param name="request">Dados do pedido a ser criado</param>
        /// <response code="201">Pedido criado com sucesso</response>
        /// <response code="400">Dados inválidos ou erro de validação</response>
        /// <response code="401">Usuário não autenticado</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto request)
        {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _service.CreateOrderAsync(request);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ErrorResponseDto(ex.Message));
            }
        }

        /// <summary>
        /// Busca um pedido específico por ID
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     GET /api/orders/1
        /// 
        /// Retorna o pedido com todas as suas ocorrências associadas.
        /// </remarks>
        /// <param name="id">ID do pedido</param>
        /// <response code="200">Pedido encontrado com sucesso</response>
        /// <response code="404">Pedido não encontrado</response>
        /// <response code="401">Usuário não autenticado</response>
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _service.GetOrderByIdAsync(id);
                return Ok(order);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(new ErrorResponseDto(ex.Message));
            }
        }

        /// <summary>
        /// Lista todos os pedidos cadastrados
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     GET /api/orders
        /// 
        /// Retorna todos os pedidos com suas respectivas ocorrências.
        /// </remarks>
        /// <response code="200">Lista de pedidos retornada com sucesso</response>
        /// <response code="401">Usuário não autenticado</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                return Ok(await _service.GetAllOrdersAsync());
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorResponseDto(ex.Message));
            }
        }

        /// <summary>
        /// Adiciona uma ocorrência a um pedido existente
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST /api/orders/1/occurrences
        ///     {
        ///        "occurrenceType": "OnDeliveryRoute",
        ///        "occurrenceTime": "2025-10-03T15:00:00"
        ///     }
        /// 
        /// **Tipos de Ocorrência Disponíveis:**
        /// - `OnDeliveryRoute` - Pedido em rota de entrega
        /// - `SuccessfullyDelivered` - Entregue com sucesso
        /// - `CustomerAbsent` - Cliente ausente
        /// - `ProductDamage` - Avaria no produto
        /// 
        /// **Regras de Negócio:**
        /// - Não permite ocorrências duplicadas do mesmo tipo em menos de 10 minutos
        /// - A segunda ocorrência é automaticamente marcada como finalizadora
        /// - Ocorrência tipo "SuccessfullyDelivered" marca o pedido como entregue
        /// </remarks>
        /// <param name="id">ID do pedido</param>
        /// <param name="request">Dados da ocorrência</param>
        /// <response code="200">Ocorrência adicionada com sucesso</response>
        /// <response code="400">Dados inválidos ou regra de negócio violada</response>
        /// <response code="404">Pedido não encontrado</response>
        /// <response code="401">Usuário não autenticado</response>
        [HttpPost("{id}/occurrences")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto),StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddOccurrence(int id, [FromBody] AddOccurrenceRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _service.AddOccurrenceAsync(id, request);

                return Ok(new { message = "Ocorrência adicionada com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return BadRequest(new ErrorResponseDto( ex.Message ));
            }
        }
        /// <summary>
        /// Exclui uma ocorrência de um pedido
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     DELETE /api/orders/1/occurrences/5
        /// 
        /// **Importante:**
        /// - Não é possível excluir ocorrências de pedidos já concluídos
        /// </remarks>
        /// <param name="id">ID do pedido</param>
        /// <param name="occurrenceId">ID da ocorrência a ser excluída</param>
        /// <response code="204">Ocorrência excluída com sucesso</response>
        /// <response code="400">Não é possível excluir a ocorrência (pedido concluído)</response>
        /// <response code="404">Pedido ou ocorrência não encontrado</response>
        /// <response code="401">Usuário não autenticado</response>
        [HttpDelete("{id}/occurrences/{occurrenceId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteOccurrence(int id, int occurrenceId)
        {
            try
            {
                await _service.DeleteOccurrenceAsync(id, occurrenceId);
                return NoContent();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ErrorResponseDto(ex.Message));
            }
        }
    }
}
