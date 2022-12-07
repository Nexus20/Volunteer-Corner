using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Volunteer_Corner.Business.Infrastructure;
using Volunteer_Corner.Business.Interfaces;
using Volunteer_Corner.Business.Interfaces.Services;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Requests.Auth;
using Volunteer_Corner.Business.Models.Results;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.Business.Services;

public class SignInService : ISignInService {

    private readonly UserManager<User> _userManager;

    private readonly JwtHandler _jwtHandler;

    public SignInService(UserManager<User> userManager, JwtHandler jwtHandler) {
        _userManager = userManager;
        _jwtHandler = jwtHandler;
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
        
        return new LoginResult() { IsAuthSuccessful = true, Token = token };
    }
}