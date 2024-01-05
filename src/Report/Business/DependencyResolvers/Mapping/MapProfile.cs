using AutoMapper;
using Entities.Concrete;
using Entities.DTOs.Params;

namespace Business.DependencyResolvers.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<ReportDetail, ReportDetailForUpsertDTO>().ReverseMap();
        }
    }
}