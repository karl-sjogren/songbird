using Songbird.Web.Models;
using AutoMapper;
using Songbird.Web.DataObjects;

namespace Songbird.Web.MappingProfiles;

public class CustomerMappingProfile : Profile {
    public CustomerMappingProfile() {
        CreateMap<Customer, CustomerDTO>();
    }
}
