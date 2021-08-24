using AutoMapper;
using Ecommerce.Model;
using Ecommerce.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Mapper
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Products, ProductDto>().ReverseMap();
        }
    }
}
