using Microsoft.Extensions.Configuration;
using Store.Core.Entities.Basket;
using Store.Core.Entities.Order;
using Store.Core.Entities.Product;
using Store.Core.Repositories.Contract;
using Store.Core.Servecies.Contract;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Core.Entities.Product.Product;

namespace Store.Services.Servecies
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration,
                              IBasketRepository basketRepository,
                              IUnitOfWork unitOfWork)
        {
            this._configuration = configuration;
            this._basketRepository = basketRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntenAsync(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];
            var basket = await _basketRepository.GetBasketAsync(BasketId);
            if (basket is null) return null;


            var shippingPrice = 0M;
            PaymentIntent paymentIntent;

            if (basket.DelivaryMethodId.HasValue)
            {
                var delivaryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(basket.DelivaryMethodId.Value);

                shippingPrice = delivaryMethod.Cost;

                if (basket.Items.Count() > 0)
                {
                    foreach (var item in basket.Items)
                    {
                        var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(item.Id);
                        if (product.Price != item.Price)
                        {
                            item.Price=product.Price;
                        }
                    }
                }

                var subTotal = basket.Items.Sum(item => item.Price * item.Quantity);
                var amout = subTotal + shippingPrice;
                var service = new PaymentIntentService();
                if (string.IsNullOrEmpty(basket.PaymentIntenId))
                {
                    var Options = new PaymentIntentCreateOptions()
                    {
                        Amount = (long)amout * 100,
                        Currency = "usd",
                        PaymentMethodTypes = new List<string>() { "card" }
                    };

                    paymentIntent = await service.CreateAsync(Options);
                    basket.PaymentIntenId = paymentIntent.Id;
                    basket.ClientSecret = paymentIntent.ClientSecret;


                }
                else
                {
                    var Options = new PaymentIntentUpdateOptions()
                    {
                        Amount = (long)amout * 100
                    };

                    paymentIntent = await service.UpdateAsync(basket.PaymentIntenId, Options);
                    basket.PaymentIntenId = paymentIntent.Id;
                    basket.ClientSecret = paymentIntent.ClientSecret;
                }

            }
                await _basketRepository.UpdateBasketAsync(basket);
                return basket;
    }
    }
}
