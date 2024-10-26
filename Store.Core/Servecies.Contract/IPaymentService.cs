using Store.Core.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Servecies.Contract
{
    public interface IPaymentService
    {

        public Task<CustomerBasket?> CreateOrUpdatePaymentIntenAsync(string BasketId);
      


    }
}
