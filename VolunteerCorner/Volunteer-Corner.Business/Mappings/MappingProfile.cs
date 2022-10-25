using AutoMapper;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.Business.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterRequest, User>();
        CreateMap<User, RegisterResult>();
    }
}