using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volunteer_Corner.Business.Exceptions;
using Volunteer_Corner.Business.Interfaces.Services;
using Volunteer_Corner.Business.Models.Enums;
using Volunteer_Corner.Business.Models.Requests.Auth;
using Volunteer_Corner.Business.Models.Requests.Users;
using Volunteer_Corner.Business.Models.Results;
using Volunteer_Corner.Data;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Entities.Identity;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Business.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(UserManager<User> userManager, IMapper mapper, ILogger<UserService> logger,
        ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<UserResult> RegisterAsync(RegisterRequest request)
    {
        if (!Enum.IsDefined(request.AccountType))
            throw new ValidationException("Invalid account type");
        
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user != null)
            throw new ValidationException($"User with such login {request.UserName} already exists");

        user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user != null)
            throw new ValidationException($"User with such email {request.Email} already exists");

        user = _mapper.Map<RegisterRequest, User>(request);
        var identityResult = await _userManager.CreateAsync(user, request.Password);
        
        if(identityResult.Errors.Any())
            throw new IdentityException(identityResult.Errors);
        
        identityResult = await _userManager.AddToRolesAsync(user, new List<string>
        {
            CustomRoles.UserRole,
            request.AccountType switch {
                AccountType.HelpSeeker => CustomRoles.HelpSeekerRole,
                AccountType.Volunteer => CustomRoles.VolunteerRole
            }
        });
        
        if(identityResult.Errors.Any())
            throw new IdentityException(identityResult.Errors);

        if (request.AccountType == AccountType.HelpSeeker)
        {
            var helpSeekerProfile = new HelpSeeker
            {
                UserId = user.Id
            };

            await _dbContext.HelpSeekers.AddAsync(helpSeekerProfile);
        }
        else if(request.AccountType == AccountType.Volunteer)
        {
            var volunteerProfile = new Volunteer
            {
                UserId = user.Id
            };

            await _dbContext.Volunteers.AddAsync(volunteerProfile);
        }

        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("User {UserId} has been successfully registered", user.Id);

        var result = _mapper.Map<User, UserResult>(user);
        return result;
    }

    public async Task<UserResult> UpdateOwnProfileAsync(string profileOwnerId, UpdateOwnProfileRequest updateProfileRequest)
    {
        var userToUpdate = await _userManager.FindByIdAsync(profileOwnerId);

        if (userToUpdate == null)
            throw new NotFoundException(nameof(User), profileOwnerId);

        _mapper.Map<UpdateOwnProfileRequest, User>(updateProfileRequest, userToUpdate);

        var identityResult = await _userManager.UpdateAsync(userToUpdate);
        
        if(identityResult.Errors.Any())
            throw new IdentityException(identityResult.Errors);
        
        _logger.LogInformation("User {UserId} has been successfully updated", profileOwnerId);
        
        var result = _mapper.Map<User, UserResult>(userToUpdate);
        return result;
    }

    public async Task<ContactsDisplayPolicy> AdjustContactsDisplaying(string userId, AdjustContactsDisplaying request)
    {
        var userToUpdate = await _userManager.FindByIdAsync(userId);

        if (userToUpdate == null)
            throw new NotFoundException(nameof(User), userId);

        userToUpdate.ContactsDisplayPolicy = request.NewPolicy;
        var identityResult = await _userManager.UpdateAsync(userToUpdate);
        
        if(identityResult.Errors.Any())
            throw new IdentityException(identityResult.Errors);
        
        _logger.LogInformation("User {UserId} has successfully updated contacts displaying policy to {NewPolicy}", userId, request.NewPolicy.ToString());
        return request.NewPolicy;
    }
}