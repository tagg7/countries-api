using AutoMapper;
using Countries.Application.Dtos;
using Countries.Domain.Entities;

namespace Countries.Web.MappingProfiles;

public class CountryMappingProfile : Profile
{
    public CountryMappingProfile()
    {
        CreateMap<Country, CountryDto>().ReverseMap();
    }
}