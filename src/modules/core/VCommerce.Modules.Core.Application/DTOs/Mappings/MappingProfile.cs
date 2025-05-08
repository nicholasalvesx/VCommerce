using AutoMapper;
using VCommerce.Modules.Core.Domain.Entities;

namespace VCommerce.Modules.Core.Application.DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDTO>().ReverseMap();
        
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ForMember(x => x.CategoryName,
            opt => opt.MapFrom(src => src.CategoryName));

        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.CategoryName!.CategoryName))
            .ReverseMap();
        
        CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.CustomerId, opt =>
                opt.MapFrom(src => src.Customer!.Id))
            .ForMember(dest => dest.Items, opt =>
                opt.MapFrom(src => src.Items)).ReverseMap();

    }
}