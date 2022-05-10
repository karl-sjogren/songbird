using Songbird.Web.Models;
using AutoMapper;
using Songbird.Web.DataObjects;

namespace Songbird.Web.MappingProfiles;

public class ApplicationMappingProfile : Profile {
    public ApplicationMappingProfile() {
        CreateMap<ApplicationLogFilter, LogSearchApplicationFilterDTO>()
            .ForMember(x => x.ApplicationName, opt => opt.MapFrom(x => x.Application.Name))
            .ForMember(x => x.CustomerName, opt => opt.MapFrom(x => x.Application.Project.Customer.Name))
            .ForMember(x => x.Environment, opt => opt.MapFrom(x => x.Environment.ToString()))
            .ForMember(x => x.FilterValue, opt => opt.MapFrom(x => x.FilterValue))
            .ForMember(x => x.ProjectName, opt => opt.MapFrom(x => x.Application.Project.Name));
    }
}
