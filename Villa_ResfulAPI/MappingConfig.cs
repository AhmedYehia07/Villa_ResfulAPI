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
            CreateMap<Villa,VillaCreateDto>().ReverseMap();
            CreateMap<Villa,VillaUpdateDto>().ReverseMap();
        }
    }
}
