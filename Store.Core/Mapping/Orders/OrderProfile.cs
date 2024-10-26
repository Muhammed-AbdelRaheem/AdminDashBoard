using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Core.Dtos.Auth;
using Store.Core.Dtos.Order;
using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Mapping.Orders
{
    public class OrderProfile:Profile
    {

        public OrderProfile(IConfiguration configuration)
        {
            CreateMap<Address, AddressDto>().ReverseMap();


            CreateMap<Order, OrderToReturned>().
                                                ForMember(d => d.DeliveryMethod,O=>O.MapFrom(S=>S.DeliveryMethod.ShortName)).
                                                ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));
          
            
            
            CreateMap<OrderItem, OrderItemDto>().ForMember(d => d.Id, O => O.MapFrom(S => S.Product.Id)).
                                                 ForMember(d => d.ProductName, O => O.MapFrom(S => S.Product.ProductName)).
                                                 ForMember(d => d.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl)).
                                                 ForMember(d => d.PictureUrl, O => O.MapFrom(new PictureUrlResolver(configuration)));


        }
    }
}
