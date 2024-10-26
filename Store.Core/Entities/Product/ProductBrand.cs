using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entities.Product
{
    public class ProductBrand : BaseEntity<int>
    {
        public string Name { get; set; }

    }
}
