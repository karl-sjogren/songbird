using Songbird.Web.Models;
using AutoMapper;
using Songbird.Web.DataObjects;

namespace Songbird.Web.MappingProfiles {
    public class UserMappingProfile : Profile {
        public UserMappingProfile() {
            CreateMap<User, UserDTO>();
        }
    }
}
