using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.Core.Dtos.Order;
using Store.Core.Entities.Order;
using Store.Core.Repositories.Contract;
using Store.Core.Servecies.Contract;
using System.Security.Claims;

namespace Store.APIs.Controllers
{

    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService,
                                IMapper mapper,
                                IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {

            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var MappedAddress =_mapper.Map<Address>(orderDto.ShippingAddress);

            var Order= await _orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, MappedAddress);

            if (Order is null)
            {
                return BadRequest(new ApiErrorResponse(400, "There Is Problem With Your Order"));
            }

            return Ok(Order);

        }



        [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet]
       public async Task<ActionResult<IEnumerable<OrderToReturned>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var orders = await _orderService.GetOrderForSpecificUserAsync(buyerEmail);
            if (orders is null) return NotFound(new ApiErrorResponse(404,"There Is No Orders For This User"));

            var MappedOrder =_mapper.Map<IEnumerable<OrderToReturned>>(orders);

            return Ok(MappedOrder) ;
        }



        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpGet("{id}")]

        public async Task<ActionResult<Order>> GetOrderByIdForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);



          var order=  await _orderService.GetOrderByIdForSpecificUserAsync(buyerEmail, id);

            if (order is null) return NotFound(new ApiErrorResponse(404,$"There is No Order With This Id:( {id} ) For This User"));

            var MappedOrder = _mapper.Map<Order,OrderToReturned>(order);

            return Ok(MappedOrder);

         }


        [HttpGet("DeliveryMethods")]  //Get :BaseUrl/Api/Orders/DeliveryMethods


        public async Task<ActionResult<IEnumerator<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods =await _unitOfWork.Repository<DeliveryMethod,int>().GetAllAsync();
            return Ok(deliveryMethods);
        } 
    }
}
