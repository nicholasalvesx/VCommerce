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
    }
}