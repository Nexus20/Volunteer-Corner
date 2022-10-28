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
        CreateMap<HelpSeeker, HelpSeekerResult>();
        CreateMap<HelpRequestDocument, HelpRequestDocumentResult>();
    }
}