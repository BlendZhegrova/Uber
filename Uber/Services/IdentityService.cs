using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Uber.Data;
using Uber.Domain;
using Uber.Options;
using Uber.Services.Interfaces;

namespace Uber.Services;

public class IdentityService : IIdentityService
{

    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly DataContext _context;

    public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings,
        TokenValidationParameters tokenValidationParameters, DataContext context)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
        _tokenValidationParameters = tokenValidationParameters;
        _context = context;
    }
    
    public async Task<AuthenticationResult> RegisterAsync(string email, string password)
    {
        var existing = await _userManager.FindByEmailAsync(email);
        if (existing != null)
        {
            return new AuthenticationResult
            {
                Errors = new[] { "User with this email already exists!" }
            };
        }

        var newUserId = Guid.NewGuid();
        var newUser = new IdentityUser
        {
            Id = newUserId.ToString(),
            Email = email,
            UserName = email,
        };
        var createdUser = await _userManager.CreateAsync(newUser, password);
        
        // await _userManager.AddClaimAsync(newUser, new Claim("tags.View","true"));
        
        if (!createdUser.Succeeded)
        {
            return new AuthenticationResult
            {
                Errors = createdUser.Errors.Select(x => x.Description)
            };
        }

        return await GenerateAuthenticationResultForUserAsync(newUser);
    }

    public Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        throw new NotImplementedException();
    }

    public Task<AuthenticationResult> RefreshTokenAsync(string requestToken, string refreshToken)
    {
        throw new NotImplementedException();
    }

    private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub,user.Email),
            new Claim(JwtRegisteredClaimNames.Sub,Guid.NewGuid().ToString()), //token indentifier
            new Claim(JwtRegisteredClaimNames.Sub,user.Email),
            new Claim("Id", user.Id)
        };

        var userClaims = await _userManager.GetClaimsAsync(user);
        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            claims.Add(new Claim("Role", role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        RefreshToken refreshToken = new()
        {
            JwtId = token.Id,
            UserId = user.Id,
            ExpiryDate = DateTime.UtcNow.AddDays(1)
        };
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
        return new AuthenticationResult()
        {
            Succes = true,
            Token = tokenHandler.WriteToken(token),
            RefreshToken = refreshToken.Token
        };
    }

    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal =
                tokenHandler.ValidateToken(token, _tokenValidationParameters, out SecurityToken validatedToken);
            if (!IsJwtWithSecurityWithValidAlgorithm(validatedToken))
            {
                return null;
            }

            return principal;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private bool IsJwtWithSecurityWithValidAlgorithm(SecurityToken validatedToken)
    {
        return (validatedToken is JwtSecurityToken jwtSecurityToken &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase));
    }
}