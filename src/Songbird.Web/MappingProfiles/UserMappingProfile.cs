using Songbird.Web.Models;
using AutoMapper;
using Songbird.Web.DataObjects;

namespace Songbird.Web.MappingProfiles {
    public class UserProfile : Profile {
        public UserProfile() {
            CreateMap<User, UserDTO>();
        }
    }
    public class FikaScheduleProfile : Profile {
        public FikaScheduleProfile() {
            CreateMap<FikaSchedule, FikaScheduleDTO>();
        }
    }
    public class FikaMatchProfile : Profile {
        public FikaMatchProfile() {
            CreateMap<FikaMatch, FikaMatchDTO>();
        }
    }
}
