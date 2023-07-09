using Microsoft.AspNetCore.Mvc;
using MoeUC.Api.Models;
using MoeUC.Core.Infrastructure.Mapping;
using MoeUC.Service.ServiceBase;
using MoeUC.Service.ServiceBase.Models;
using MoeUC.Service.ThirdParties;

namespace MoeUC.Api.Controllers;

public class OauthController : BaseApiController
{
    private readonly GitHubService _gitHubService;
    public OauthController(WorkContext workContext, GitHubService gitHubService) : base(workContext)
    {
        _gitHubService = gitHubService;
    }

    /// <summary>
    /// Register user by GitHub Oauth
    /// </summary>
    /// <param name="code">code returned by github</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<CommonJsonResponse> RegisterByGitHub(string code)
    {
        var user = await _gitHubService.InitUserByGitHub(code);

        return _workContext.Successed(user.MapTo<UserRegisterResponseModel>());
    }
}