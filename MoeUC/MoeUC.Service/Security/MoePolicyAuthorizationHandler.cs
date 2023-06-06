using Microsoft.AspNetCore.Authorization;
using MoeUC.Core.Infrastructure.Dependency;
using MoeUC.Service.ServiceBase.Models;

namespace MoeUC.Service.Security;

public class MoePolicyAuthorizationHandler : AuthorizationHandler<MoePolicyRequirement>, IScoped
{
    private readonly WorkContext _workContext;

    public MoePolicyAuthorizationHandler(WorkContext workContext)
    {
        _workContext = workContext;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MoePolicyRequirement requirement)
    {
        // todo : Implement
        var userId = _workContext.CurrentUserInfo.Id;
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}

public class MoePolicyRequirement : IAuthorizationRequirement
{
    public string? Name { get; set; }
}