using Songbird.Web.Models;
using AutoMapper;
using Songbird.Web.DataObjects;

namespace Songbird.Web.MappingProfiles;

public class PlanningBoardMappingProfile : Profile {
    public PlanningBoardMappingProfile() {
        CreateMap<PlanningBoard, PlanningBoardDTO>();
    }
}
