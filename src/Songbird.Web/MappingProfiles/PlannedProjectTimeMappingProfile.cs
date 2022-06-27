using Songbird.Web.Models;
using AutoMapper;
using Songbird.Web.DataObjects;

namespace Songbird.Web.MappingProfiles;

public class PlannedProjectTimeMappingProfile : Profile {
    public PlannedProjectTimeMappingProfile() {
        CreateMap<PlannedProjectTime, PlannedProjectTimeDTO>();
    }
}
