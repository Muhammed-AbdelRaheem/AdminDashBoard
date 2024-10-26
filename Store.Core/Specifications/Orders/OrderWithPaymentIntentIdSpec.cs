using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.Orders
{
    public class OrderWithPaymentIntentIdSpec:BaseSpecifications<Order,int>
    {
        public OrderWithPaymentIntentIdSpec(string paymentIntentId):base (O=>O.PaymentIntentId==paymentIntentId)
        {
     
        }


    }
}
