using Store.Core.Entities.Basket;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Dtos
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }

        public List<BasketItem> Items { get; set; }


        public string? PaymentIntenId { get; set; }
        public string? ClientSecret { get; set; }

        public int? DelivaryMethodId { get; set; }

    }
}
