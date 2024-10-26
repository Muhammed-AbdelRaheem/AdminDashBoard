using AutoMapper;
using Store.Core.Entities.Product;
using Store.DashBoard.Models;

namespace Store.DashBoard.Helpers
{
    public class MapsProfile:Profile
    {
        public MapsProfile()
        {
            CreateMap<Product,ProductViewModel>().ReverseMap();
        }
    }
}
