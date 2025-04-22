using AutoMapper;
using VCommerce.Api.Models;

namespace VCommerce.Api.DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDTO>().ReverseMap();
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Product, ProductDTO>().ReverseMap();
        CreateMap<Product, ProductDTO>()
            .ForMember(x => x.CategoryName, opt => 
                opt.MapFrom(src => src.Category!.Name));
        
        CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.CustomerId, opt =>
                opt.MapFrom(src => src.Customer!.Id))
            .ForMember(dest => dest.Items, opt =>
                opt.MapFrom(src => src.Items));
        
        CreateMap<OrderDTO, Order>()
            .ForMember(dest => dest.Customer, opt => opt.Ignore()) // opcional: pode buscar no banco
            .ForMember(dest => dest.Items, opt =>
                opt.MapFrom(src => src.Items));

    }
}