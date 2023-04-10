using AutoMapper;
using ZipCodeRadius.DTOs;
using ZipCodeRadius.Model;

namespace ZipCodeRadius.Profiles;

public class zipCodeProfile : Profile
{
    public zipCodeProfile()
    {
        CreateMap<ZipCodeData, zipCodeInfoDto>();
        CreateMap<ZipCodeData, zipCodeBasicInfoDto>()
            .ForMember(dest => dest.cityStateZipcode, src => src.MapFrom(x => $"{x.cityName}, {x.stateAbbr} {x.ZipCode}"));
        CreateMap<Blood, bloodTypesDto>();

    }
}
