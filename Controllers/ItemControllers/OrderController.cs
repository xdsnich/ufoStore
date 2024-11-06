using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ufoShopBack.CRUDoperations;
using ufoShopBack.Data;
using ufoShopBack.Data.Entities;
using ufoShopBack.DTOs;
using ufoShopBack.Services;
namespace ufoShopBack.Controllers.ItemControllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : Controller
    {
        private readonly OrdersCRUD _orderCRUD;
        private readonly Context _context;
        private readonly ShowOrderService _showOrderService;
        private readonly CreateOrderService _createOrderService;

        public OrderController(OrdersCRUD orderCRUD, Context context, ShowOrderService showOrderService, CreateOrderService createOrderService) 
        { 
            _orderCRUD = orderCRUD;
            _context = context;
            _showOrderService = showOrderService;
            _createOrderService = createOrderService;
        }
        [Authorize(Roles = "Moderator,Admin")]
        [HttpGet("getorder")]
        public async Task<IActionResult> GetOrder()
        {
            var orders = await _orderCRUD.GetAsync();
            return Ok(orders);
        }
        [Authorize]
        [HttpGet("show/{id}")]
        public async Task<IActionResult> ShowOrderWithItems(int id)
        {
            var orders = await _showOrderService.ShowOrderAsync(id);
            return Ok(orders);
        }
        [Authorize]
        [HttpPost("create/{userId}")]
        public async Task<IActionResult> CreateOrderWithItems(int userId, List<OrderItemDTO> orderItems) 
        {
            var createdOrder = await _createOrderService.OrderCreationAsync(userId, orderItems);
            if (createdOrder == null) {
                return BadRequest("entered invalid data");
            }
            return Created();
        }
    }
}
