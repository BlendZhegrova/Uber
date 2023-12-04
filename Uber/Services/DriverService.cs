using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Uber.Contract.V1.Requests;
using Uber.Domain;
using Uber.Repositories.Interface;
using Uber.Services.Interfaces;

namespace Uber.Services;

public class DriverService : IDriverService
{
    private IIdentityService _identityService;
    private readonly UserManager<IdentityUser> _userManager;
    public DriverService(IIdentityService identityService, UserManager<IdentityUser> userManager)
    {
        _identityService = identityService;
        _userManager = userManager;
    }

    public async Task<AuthenticationResult> RegisterDriverAsync(DriverRegisterRequest request)
    {
        var existing = await _userManager.FindByEmailAsync(request.Email);
        if (existing != null)
        {
            return new AuthenticationResult()
            {
                Errors = new[] { "There is already a driver with this email!" }
            };
        }

        var driverId = new Guid();
        Driver driver = new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            RegistrationDate = DateTime.Now,
            State = request.State,
            Ssn = request.Ssn,
            Dln = request.Dln,
            LicensePlate = request.LicensePlate
        };

        var createdDriver = await _userManager.CreateAsync(driver);

        if (!createdDriver.Succeeded)
        {
            return new AuthenticationResult
            {
                Errors = createdDriver.Errors.Select(x => x.Description)
            };
        }

        return await _identityService.GenerateAuthenticationResultForUserAsync(driver);
    }

    public async Task<AuthenticationResult> LoginAsync(DriverLoginRequest request)
    {
        var driver =  await _userManager.FindByEmailAsync(request.Email);
        if (driver == null)
        {
            return new AuthenticationResult
            {
                Errors = new[] { "Driver does not exist!" }
            };
        }
        
        var userHasValidPassword = await _userManager.CheckPasswordAsync(driver, request.Password);

        if (!userHasValidPassword)
        {
            return new AuthenticationResult
            {
                Errors = new[] { "Driver/Password combination is wrong" }
            };
        }

        return await _identityService.GenerateAuthenticationResultForUserAsync(driver);
    }
}