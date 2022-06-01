using Songbird.Web.Models;
using AutoMapper;
using Songbird.Web.DataObjects;

namespace Songbird.Web.MappingProfiles;

public class ScheduledStatusMappingProfile : Profile {
    public ScheduledStatusMappingProfile() {
        CreateMap<ScheduledStatus, ScheduledStatusDTO>();
    }
}
