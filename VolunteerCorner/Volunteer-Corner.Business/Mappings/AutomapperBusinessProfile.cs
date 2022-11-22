using AutoMapper;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results;
using Volunteer_Corner.Business.Models.Results.HelpRequests;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.Business.Mappings;

public class AutomapperBusinessProfile : Profile
{
    public AutomapperBusinessProfile()
    {
        CreateMap<RegisterRequest, User>();
        CreateMap<User, RegisterResult>();

        CreateMap<HelpRequest, HelpRequestResult>();
        CreateMap<HelpSeeker, HelpSeekerResult>()
            .ForMember(x => x.FirstName, o => o.MapFrom(s => s.User.FirstName))
            .ForMember(x => x.LastName, o => o.MapFrom(s => s.User.LastName))
            .ForMember(x => x.Patronymic, o => o.MapFrom(s => s.User.Patronymic))
            .ForMember(x => x.Email, o => o.MapFrom(s => s.User.Email))
            .ForMember(x => x.Phone, o => o.MapFrom(s => s.User.PhoneNumber));

        CreateMap<HelpRequestDocument, HelpRequestDocumentResult>();
        CreateMap<CreateHelpRequestRequest, HelpRequest>();
        CreateMap<UpdateHelpRequestRequest, HelpRequest>();
    }
}