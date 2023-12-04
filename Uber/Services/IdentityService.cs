using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Uber.Contract.V1.Requests;
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

    public async Task<AuthenticationResult> RefreshTokenAsync(string requestToken, string refreshToken)
    {
        var validatedToken = GetPrincipalFromToken(requestToken);
        if (validatedToken == null)
        {
            return new AuthenticationResult()
            {
                Errors = new[] { "Invalid token" }
            };
        }

        var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);
        if (expiryDateUtc > DateTime.Now)
        {
            return new AuthenticationResult()
            {
                Errors = new[] { "This token has not expired yet." }
            };
        }

        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        var storedRefreshTokens = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);
        if (storedRefreshTokens == null)
        {
            return new AuthenticationResult
            {
                Errors = new[] { "This token hasnt expired yet" }
            };
        }
        if (DateTime.UtcNow > storedRefreshTokens.ExpiryDate)
        {
            return new AuthenticationResult
            {
                Errors = new[] { "This token has expired!" }
            };
        }
        if (storedRefreshTokens.Invalidated)
        {
            return new AuthenticationResult
            {
                Errors = new[] { "This token has been invalidated" }
            };
        }
        if (storedRefreshTokens.Used)
        {
            return new AuthenticationResult
            {
                Errors = new[] { "This token has been used" }
            };
        }
        if (storedRefreshTokens.JwtId != jti)
        {
            return new AuthenticationResult
            {
                Errors = new[] { "This token does not match this JWT"}
            };
        }

        storedRefreshTokens.Used = true;
        _context.RefreshTokens.Update(storedRefreshTokens);
        await _context.SaveChangesAsync();
        var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type=="id").Value);
        return await GenerateAuthenticationResultForUserAsync(user);
    }

    public async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub,user.Email),
            new Claim(JwtRegisteredClaimNames.Sub,Guid.NewGuid().ToString()), //token indentifier
            new Claim(JwtRegisteredClaimNames.Sub,user.Email),
            new Claim("Id", user.Id.ToString())
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
            UserId = user.Id.ToString(),
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