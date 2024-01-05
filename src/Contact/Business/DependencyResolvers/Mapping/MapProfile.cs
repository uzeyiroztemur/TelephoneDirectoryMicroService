using AutoMapper;
using Entities.Concrete;
using Entities.DTOs.Params;

namespace Business.DependencyResolvers.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Person, PersonForUpsertDTO>().ReverseMap();
            CreateMap<PersonContactInfo, PersonContactInfoForUpsertDTO>().ReverseMap();
        }
    }
}