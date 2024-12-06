using AutoMapper;
using Villa_ResfulAPI.Models;
using Villa_ResfulAPI.Models.DTO;

namespace Villa_ResfulAPI
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa,VillaDto>().ReverseMap();
            CreateMap<VillaDto,VillaCreateDto>().ReverseMap();
            CreateMap<VillaDto,VillaUpdateDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreateDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDto>().ReverseMap();
        }
    }
}
