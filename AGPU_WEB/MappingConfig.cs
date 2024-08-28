using AGPU_WEB.Models.DTO;
using AutoMapper;

namespace AGPU_WEB
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //CreateMap<AGPU, AGPUDTO>();
            CreateMap<AGPUDTO, AGPUCreateDTO>().ReverseMap();
            CreateMap<AGPUDTO, AGPUUpdateDTO>().ReverseMap();
        }
    }
}
