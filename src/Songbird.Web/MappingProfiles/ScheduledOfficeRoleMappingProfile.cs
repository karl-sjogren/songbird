using Songbird.Web.Models;
using AutoMapper;
using Songbird.Web.DataObjects;

namespace Songbird.Web.MappingProfiles;

public class ScheduledOfficeRoleMappingProfile : Profile {
    public ScheduledOfficeRoleMappingProfile() {
        CreateMap<ScheduledOfficeRole, ScheduledOfficeRoleDTO>();
    }
}
