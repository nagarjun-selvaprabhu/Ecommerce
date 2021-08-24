using AutoMapper;
using OrderEndpoint.Model;
using OrderEndpoint.Model.Dtos;

namespace OrderEndpoint.Mapper
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
