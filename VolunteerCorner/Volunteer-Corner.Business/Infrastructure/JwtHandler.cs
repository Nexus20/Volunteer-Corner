using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.Business.Infrastructure;

public class JwtHandler {
    
    private readonly IConfigurationSection _jwtSettings;
    private readonly UserManager<User> _userManager;

    public JwtHandler(IConfiguration configuration, UserManager<User> userManager) {
        _userManager = userManager;
        _jwtSettings = configuration.GetSection("JwtSettings");
    }
    
    public SigningCredentials GetSigningCredentials() {
        
        var key = Encoding.UTF8.GetBytes(_jwtSettings["securityKey"]);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
    
    public async Task<List<Claim>> GetClaimsAsync(User user) {
        
        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };
        
        var roles = await _userManager.GetRolesAsync(user);
        
        foreach (var role in roles) {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        return claims;
    }
    
    public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims) {
        
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtSettings["validIssuer"],
            audience: _jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
            signingCredentials: signingCredentials);
        
        return tokenOptions;
    }
}