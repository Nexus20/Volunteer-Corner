using AutoMapper;
using Volunteer_Corner.Business.Models.Requests.Auth;
using Volunteer_Corner.Business.Models.Requests.HelpRequests;
using Volunteer_Corner.Business.Models.Requests.Users;
using Volunteer_Corner.Business.Models.Results.Abstract;
using Volunteer_Corner.Business.Models.Results.HelpProposals;
using Volunteer_Corner.Business.Models.Results.HelpRequests;
using Volunteer_Corner.Business.Models.Results.HelpSeekers;
using Volunteer_Corner.Business.Models.Results.Users;
using Volunteer_Corner.Business.Models.Results.Volunteers;
using Volunteer_Corner.Data.Dtos;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.Business.Mappings;

public class AutomapperBusinessProfile : Profile
{
    public AutomapperBusinessProfile()
    {
        CreateMap<RegisterRequest, User>();
        CreateMap<UpdateOwnProfileRequest, User>();
        CreateMap<User, UserResult>();

        CreateMap<HelpRequest, HelpRequestResult>();
        // .ForMember(x => x.AdditionalDocuments, o => o.Condition(s => s.AdditionalDocuments?.Any() == true));
        
        CreateMap<HelpRequest, HelpRequestWithHelpResponsesResult>();
        
        CreateMap<HelpSeeker, HelpSeekerResult>()
            .ForMember(x => x.ContactsDisplayPolicy, o => o.MapFrom(s => s.User.ContactsDisplayPolicy))
            .ForMember(x => x.FirstName, o => o.MapFrom(s => s.User.FirstName))
            .ForMember(x => x.LastName, o => o.MapFrom(s => s.User.LastName))
            .ForMember(x => x.Patronymic, o => o.MapFrom(s => s.User.Patronymic))
            .ForMember(x => x.Email, o => o.MapFrom(s => s.User.Email))
            .ForMember(x => x.Phone, o => o.MapFrom(s => s.User.PhoneNumber));

        CreateMap<HelpRequestDocument, HelpRequestDocumentResult>();
        CreateMap<CreateHelpRequestRequest, HelpRequest>();
        CreateMap<UpdateHelpRequestRequest, HelpRequest>();
        
        CreateMap<HelpProposal, HelpProposalResult>();
        CreateMap<Volunteer, VolunteerResult>()
            .ForMember(x => x.ContactsDisplayPolicy, o => o.MapFrom(s => s.User.ContactsDisplayPolicy))
            .ForMember(x => x.FirstName, o => o.MapFrom(s => s.User.FirstName))
            .ForMember(x => x.LastName, o => o.MapFrom(s => s.User.LastName))
            .ForMember(x => x.Patronymic, o => o.MapFrom(s => s.User.Patronymic))
            .ForMember(x => x.Email, o => o.MapFrom(s => s.User.Email))
            .ForMember(x => x.Phone, o => o.MapFrom(s => s.User.PhoneNumber));

        CreateMap<HelpRequestResponse, HelpRequestResponseResult>();

        CreateMap<Role, RoleResult>();
        CreateMap<User, ProfileResult>()
            .ForMember(x => x.Roles, o => o.MapFrom(s => s.UserRoles.Select(ur => ur.Role)));

        CreateMap(typeof(PageDto<>), typeof(PageResult<>));
    }
}