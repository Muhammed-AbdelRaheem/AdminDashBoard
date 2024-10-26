using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.Core.Dtos;
using Store.Core.Servecies.Contract;

namespace Store.APIs.Controllers
{

    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            this._paymentService = paymentService;
        }


        [HttpPost("basketId")]

        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdateIntent(string basketId)
        {

            var Basket = await _paymentService.CreateOrUpdatePaymentIntenAsync(basketId);


            if (Basket is null) return BadRequest(new ApiErrorResponse(400, "There Is Problem With Your Basket"));


            return Ok(Basket);
        }




        



    }
}
