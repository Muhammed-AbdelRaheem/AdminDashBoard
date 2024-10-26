using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.Orders
{
    public class OrderSpecifications:BaseSpecifications<Order,int>
    {

        public OrderSpecifications(string Email ):base(O=>O.BuyerEmail==Email)
        {

            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O=>O.Items);
            AddOrderByDesc(O=>O.OrderDate);
            
        }

        public OrderSpecifications(string Email, int Id) : base( O => O.Id==Id &&  O.BuyerEmail == Email )
        {

            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);




        }


    }
}
