﻿using Store.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.Products
{
    public class ProductSpecification : BaseSpecifications<Product, int>
    {

        public ProductSpecification(int id) : base(P => P.Id == id)
        {
            ApplyIncludes();
        }

        public ProductSpecification(ProductSpecParams productSpec) : base(

            P =>
            (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search))
            &&
            (!productSpec.brandId.HasValue || productSpec.brandId == P.BrandId)
            &&
            (!productSpec.typeId.HasValue || productSpec.typeId == P.TypeId)

                                                                                  )
        {
            if (!string.IsNullOrEmpty(productSpec.sort))
            {
                switch (productSpec.sort)
                {

                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;

                    default:
                        AddOrderBy(P => P.Name);
                        break;

                }

            }
            else
            {
                AddOrderBy(P => P.Name);

            } 


            ApplyPagination(productSpec.PageSize * (productSpec.pageIndex - 1), productSpec.PageSize);

            ApplyIncludes();

        }


        private void ApplyIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);

        }
    }
}
