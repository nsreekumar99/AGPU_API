using AGPU_API.Models;
using AGPU_API.Models.DTO;
using AutoMapper;

namespace AGPU_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<AGPU, AGPUDTO>();
            CreateMap<AGPUDTO, AGPU>();

            CreateMap<AGPU, AGPUCreateDTO>().ReverseMap();
            CreateMap<AGPU, AGPUUpdateDTO>().ReverseMap();
        }
    }
}
