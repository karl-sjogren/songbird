using Songbird.Web.Models;
using AutoMapper;
using Songbird.Web.DataObjects;

namespace Songbird.Web.MappingProfiles;

public class UserScheduleMappingProfile : Profile {
    public UserScheduleMappingProfile() {
        CreateMap<UserSchedule, UserScheduleDTO>();
    }
}
