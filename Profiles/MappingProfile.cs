using AutoMapper;
using ProductQrApi.DTOs;
using ProductQrApi.Entities;

namespace ProductQrApi.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductResponseDto>()
            .ForMember(
                dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category!.Name)
            );
    }
}