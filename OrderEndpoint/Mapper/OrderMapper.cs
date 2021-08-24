using AutoMapper;
using OrderEndpoint.Model;
using OrderEndpoint.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
