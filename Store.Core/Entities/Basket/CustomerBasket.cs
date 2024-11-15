﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entities.Basket
{
    public class CustomerBasket
    {

        public CustomerBasket(string id)
        {
            Id = id;

        }
        public string Id { get; set; }

        public List<BasketItem> Items { get; set; }


        public string? PaymentIntenId { get; set; }
        public string? ClientSecret { get; set; }

        public int? DelivaryMethodId { get; set; }
    }
}
