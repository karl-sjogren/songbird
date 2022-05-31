using Songbird.Web.Models;
using AutoMapper;
using Songbird.Web.DataObjects;

namespace Songbird.Web.MappingProfiles;

public class ProjectMappingProfile : Profile {
    public ProjectMappingProfile() {
        CreateMap<Project, ProjectDTO>();
    }
}
