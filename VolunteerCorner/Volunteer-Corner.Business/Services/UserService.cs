using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volunteer_Corner.Business.Exceptions;
using Volunteer_Corner.Business.Interfaces;
using Volunteer_Corner.Business.Models.Enums;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results;
using Volunteer_Corner.Data;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.Business.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, ILogger<UserService> logger, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<RegisterResult> RegisterAsync(RegisterRequest request)
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
            var helpSeekerProfile = new HelpSeeker
            {
                UserId = user.Id
            };

            await _dbContext.HelpSeekers.AddAsync(helpSeekerProfile);
        }

        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("User {UserId} has been successfully registered", user.Id);

        var result = _mapper.Map<User, RegisterResult>(user);
        return result;
    }
}