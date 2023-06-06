using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoeUC.Service.ServiceBase.Models;

namespace MoeUC.Service.Security;

public class MoeJwtAuthenticationHandler : AuthenticationHandler<MoeAuthenticationOptions>
{
    private readonly WorkContext _workContext;
    
    public MoeJwtAuthenticationHandler(IOptionsMonitor<MoeAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, WorkContext workContext) : base(options, logger, encoder, clock)
    {
        _workContext = workContext;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // todo
        
        // success
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _workContext.CurrentUserInfo.Id.ToString())
        };
        var identity = new ClaimsIdentity(claims, nameof(MoeJwtAuthenticationHandler));
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}