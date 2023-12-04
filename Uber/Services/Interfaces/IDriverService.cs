using Uber.Contract.V1.Requests;
using Uber.Domain;

namespace Uber.Services.Interfaces;

public interface IDriverService
{
    Task<AuthenticationResult> RegisterDriverAsync(DriverRegisterRequest request);
    Task<AuthenticationResult> LoginAsync(DriverLoginRequest request);
}