using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MoeUC.Core.Infrastructure.StartupConfigs;
using MoeUC.Service.Security;

namespace MoeUC.Api.StartUps;

public class AuthStartup : IMoeStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {


        //services.AddAuthorizationCore();
        services.AddAuthentication(SecurityConstants.MoeSchemeName)
            .AddScheme<MoeAuthenticationOptions, MoeJwtAuthenticationHandler>(SecurityConstants.MoeSchemeName, c => { });

        services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, MoePolicyAuthorizationHandler>();
    }

    public void Configure(IApplicationBuilder application)
    {

        application.UseAuthentication();
        application.UseAuthorization();
    }

    public int Order => 0;
}