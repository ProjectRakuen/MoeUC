using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MoeUC.Core.Infrastructure.Dependency;

namespace MoeUC.Service.Security;

public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider,IScoped
{
    private readonly AuthorizationOptions _options;
    private static readonly object Locker = new object();
    
    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, AuthorizationOptions options1) : base(options)
    {
        _options = options1;
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (policy != null)
            return policy;
        policy = new AuthorizationPolicyBuilder()
            .AddRequirements(new MoePolicyRequirement() { Name = policyName })
            .Build();
        lock (Locker)
        {
            _options.AddPolicy(policyName, policy);
        }

        return policy;
    }
}