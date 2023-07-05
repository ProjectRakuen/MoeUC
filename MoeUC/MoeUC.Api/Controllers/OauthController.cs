using Microsoft.AspNetCore.Mvc;
using MoeUC.Service.ServiceBase;
using MoeUC.Service.ServiceBase.Models;

namespace MoeUC.Api.Controllers;

public class OauthController : BaseApiController
{
    public OauthController(WorkContext workContext) : base(workContext)
    {
    }

    /// <summary>
    /// Register user by GitHub Oauth
    /// </summary>
    /// <param name="code">code returned by github</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<CommonJsonResponse> RegisterByGitHub(string code)
    {
        throw new NotImplementedException();
    }
}