using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Mapping
{
   
        public class ProductProfile : Profile
        {
            public ProductProfile()
            {
                CreateMap<Product, ProductDto>();

                CreateMap<CreateProductDto, Product>();

                CreateMap<UpdateProductDto, Product>();
            }
        }
    }
