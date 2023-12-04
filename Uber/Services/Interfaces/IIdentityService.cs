using Microsoft.AspNetCore.Identity;
using Uber.Contract.V1.Requests;
using Uber.Domain;

namespace Uber.Services.Interfaces;

public interface IIdentityService
{
    Task<AuthenticationResult> RefreshTokenAsync(string requestToken, string refreshToken);
    Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(IdentityUser user);
}