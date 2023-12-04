using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uber.Contract.V1;
using Uber.Contract.V1.Requests;
using Uber.Contract.V1.Responses;
using Uber.Services.Interfaces;

namespace Uber.Controllers;


public class DriverController : Controller
{
     private readonly IDriverService _driverService;
     private readonly ILogger<DriverController> _logger;
     public DriverController(IDriverService driverService, ILogger<DriverController> logger)
     {
          _driverService = driverService;
          _logger = logger;
     }

     [HttpPost(ApiRoutes.Driver.Register)]
     public async Task<IActionResult> Register([FromBody] DriverRegisterRequest request)
     {
          if (!ModelState.IsValid)
          {
               return BadRequest(new AuthFailedResponse
               {
                    Errors = ModelState.Values.SelectMany(x=> x.Errors.Select(y=> y.ErrorMessage))
               });
          }
          var authResponse = await _driverService.RegisterDriverAsync(request);
          if (!authResponse.Succes)
          {
               return BadRequest(new AuthFailedResponse()
               {
                    Errors = authResponse.Errors
               });
          }
          return Ok(new AuthSuccesResponse()
          {
               Token = authResponse.Token,
               RefreshToken = authResponse.RefreshToken
          });
     }

     [HttpPost(ApiRoutes.Driver.Login)]
     public async Task<IActionResult> Login([FromBody] DriverLoginRequest request)
     {
          
          if (!ModelState.IsValid)
          {
               return BadRequest(new AuthFailedResponse()
               {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage))
               });
               
          }

          var authResponse = await _driverService.LoginAsync(request);
          return Ok(new AuthSuccesResponse()
          {
               Token = authResponse.Token,
               RefreshToken = authResponse.RefreshToken
          });
          
     }
}