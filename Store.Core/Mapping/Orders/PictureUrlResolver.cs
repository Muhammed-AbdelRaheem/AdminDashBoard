using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Core.Dtos.Order;
using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Mapping.Orders
{
    public class PictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public PictureUrlResolver(IConfiguration configuration)
        {
           _configuration = configuration;
        }


        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {

            if (! string.IsNullOrEmpty(source.Product.PictureUrl))
            {

                return $"{_configuration["BaseUrl"]}{source.Product.PictureUrl}";
            }
            return string.Empty;

        }
    }
}
