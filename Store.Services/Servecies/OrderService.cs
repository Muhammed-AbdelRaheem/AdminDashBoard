using Microsoft.AspNetCore.Http.HttpResults;
using Store.Core.Dtos;
using Store.Core.Entities.Order;
using Store.Core.Entities.Product;
using Store.Core.Repositories.Contract;
using Store.Core.Servecies.Contract;
using Store.Core.Specifications.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Servecies
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository,
                             IUnitOfWork unitOfWork,
                             IPaymentService paymentService)
        {
            this._basketRepository = basketRepository;
            this._unitOfWork = unitOfWork;
            this._paymentService = paymentService;
        }



        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress)
        {
            // 1.Get Basket From Basket Repo

            var basket = await _basketRepository.GetBasketAsync(BasketId);

            //2.Get Selected Items at Basket From Product Repo
            var orderItems = new List<OrderItem>();

            if (basket?.Items.Count() > 0)
            {

                foreach (var item in basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(item.Id);

                    var ProductItemorder = new ProductItemOrder(Product.Id, Product.Name, Product.PictureUrl);
                    var orderItrem = new OrderItem(ProductItemorder, Product.Price, item.Quantity);

                    orderItems.Add(orderItrem);

                }

            }
            //3.Calculate SubTotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            //4.Get Delivery Method From DeliveryMethod Repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(DeliveryMethodId);


            var spec = new OrderWithPaymentIntentIdSpec(basket.PaymentIntenId);

            var exOrder =await _unitOfWork.Repository<Order,int>().GetEntityWithSpecAsync(spec);

            if (exOrder is not null)
            {
                _unitOfWork.Repository<Order, int>().Delete(exOrder);
               await _paymentService.CreateOrUpdatePaymentIntenAsync(BasketId);

            }
            //5.Create Order

            var order = new Order(BuyerEmail, ShippingAddress, deliveryMethod, orderItems, subTotal ,basket.PaymentIntenId);

            //6.Add Order Locally
            await _unitOfWork.Repository<Order,int>().AddAsync(order);

            //7.Save Order To Database[ToDo]
            var Result =  await _unitOfWork.CompleteAsync();


            if (Result <= 0) return null ;
            
            return order;

        }



        public async Task<IEnumerable<Order>> GetOrderForSpecificUserAsync(string BuyerEmail)
        {
            var spec = new OrderSpecifications(BuyerEmail);

            var orders= await _unitOfWork.Repository<Order,int>().GetAllWithSpecAsync(spec);

            return orders;


        }


        public async Task<Order?> GetOrderByIdForSpecificUserAsync(string BuyerEmail, int OrderId)
        {

            var spec = new OrderSpecifications(BuyerEmail, OrderId);

            var order =await _unitOfWork.Repository<Order,int>().GetEntityWithSpecAsync(spec);


            return order;

        }
    }
}
