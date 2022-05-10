using Songbird.Web.Models;
using AutoMapper;
using Songbird.Web.DataObjects;

namespace Songbird.Web.MappingProfiles;

public class FikaScheduleMappingProfile : Profile {
    public FikaScheduleMappingProfile() {
        CreateMap<FikaSchedule, FikaScheduleDTO>();
    }
}
