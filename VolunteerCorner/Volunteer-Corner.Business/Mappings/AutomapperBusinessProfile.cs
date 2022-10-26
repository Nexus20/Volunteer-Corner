using AutoMapper;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.Business.Mappings;

public class AutomapperBusinessProfile : Profile
{
    public AutomapperBusinessProfile()
    {
        CreateMap<RegisterRequest, User>();
        CreateMap<User, RegisterResult>();
    }
}