using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Volunteer_Corner.Business.Infrastructure;
using Volunteer_Corner.Business.Interfaces.Services;
using Volunteer_Corner.Business.Models.Requests.Auth;
using Volunteer_Corner.Business.Models.Results.Auth;
using Volunteer_Corner.Business.Models.Results.Users;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.Business.Services;

public class SignInService : ISignInService {

    private readonly UserManager<User> _userManager;
    private readonly JwtHandler _jwtHandler;
    private readonly IMapper _mapper;

    public SignInService(UserManager<User> userManager, JwtHandler jwtHandler, IMapper mapper) {
        _userManager = userManager;
        _jwtHandler = jwtHandler;
        _mapper = mapper;
    }

    public async Task<LoginResult> SignInAsync(LoginRequest request) {
        
        
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password)) {
            return new LoginResult() {
                ErrorMessage = "Invalid Authentication"
            };
        }

        var signingCredentials = _jwtHandler.GetSigningCredentials();
        var claims = await _jwtHandler.GetClaimsAsync(user);
        var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        var userInfo = _mapper.Map<User, UserResult>(user);
        
        return new LoginResult()
        {
            IsAuthSuccessful = true,
            Token = token,
            UserInfo = userInfo
        };
    }
}