using Songbird.Web.Models;
using AutoMapper;
using Songbird.Web.DataObjects;

namespace Songbird.Web.MappingProfiles {
    public class FikaMatchMappingProfile : Profile {
        public FikaMatchMappingProfile() {
            CreateMap<FikaMatch, FikaMatchDTO>();
        }
    }
}
