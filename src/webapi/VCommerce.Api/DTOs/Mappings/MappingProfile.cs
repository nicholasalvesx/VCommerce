using AutoMapper;
using VCommerce.Api.Models;

namespace VCommerce.Api.DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDTO>().ReverseMap();
        
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Category, CategoryDTO>().ForMember(x => x.CategoryName,
            opt => opt.MapFrom(src => src.CategoryName));

        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category!.CategoryName));
        
        CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.CustomerId, opt =>
                opt.MapFrom(src => src.Customer!.Id))
            .ForMember(dest => dest.Items, opt =>
                opt.MapFrom(src => src.Items)).ReverseMap();

    }
}