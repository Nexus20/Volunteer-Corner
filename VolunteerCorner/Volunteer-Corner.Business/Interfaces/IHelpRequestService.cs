﻿using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results.HelpRequests;

namespace Volunteer_Corner.Business.Interfaces;

public interface IHelpRequestService
{
    Task<List<HelpRequestResult>> GetAllHelpRequests(GetAllHelpRequestsRequest request);
    Task<HelpRequestResult> GetHelpRequestById(string requestId);
}