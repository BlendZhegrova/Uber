using System.Security.Principal;
using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Uber.Data;
using Uber.Filters;
using Uber.Options;
using Uber.Services;
using Uber.Services.Interfaces;

namespace Uber.Installers;

public class MvcInstallers : IInstaller
{
    public void InstallServices(IConfiguration configuration, IServiceCollection services)
    {
        var jwtSettings = new JwtSettings();
        var token = jwtSettings.Secret;
        configuration.Bind(nameof(jwtSettings), jwtSettings);
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();
        services.AddSession();
        services.AddSingleton(jwtSettings);
        services.AddControllersWithViews();
        services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add<ValidationFilter>();
            }
        );
        services.AddFluentValidation(fv =>
        {
            fv.RegisterValidatorsFromAssemblyContaining<Program>();
        });
            
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = true
        };

        services.AddSingleton(tokenValidationParameters);
        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        ).AddJwtBearer(x =>
        {
            x.SaveToken = true;
            x.TokenValidationParameters = tokenValidationParameters;
        });

        #region Authorization Policies

        // services.AddAuthorization(options =>
        // {
        //     options.AddPolicy("MustBeAVeteranDriver");
        // });

        #endregion

        //services.AddSingleton<IAuthorizationHandler, IsAVeteranDriverHandler>();

        services.AddHttpContextAccessor();

        services.AddSingleton<IUriService>(provider =>
        {
            var accesor = provider.GetRequiredService<IHttpContextAccessor>();
            var request = accesor.HttpContext.Request;
            var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");
            return new UriService(absoluteUri);
        });
    }
}