using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoeUC.Api.Models;
using MoeUC.Service.Auth;
using MoeUC.Service.ServiceBase;
using MoeUC.Service.ServiceBase.Models;
using MoeUC.Service.User;

namespace MoeUC.Api.Controllers;

public class LoginController : BaseApiController
{
    private readonly ILogger<LoginController> _logger;
    private readonly AuthHelper _authHelper;
    private readonly MoeUserService _userService;
    private readonly WorkContext _workContext;

    public LoginController(ILogger<LoginController> logger, AuthHelper authHelper, MoeUserService userService, WorkContext workContext)
    {
        this._logger = logger;
        _authHelper = authHelper;
        _userService = userService;
        _workContext = workContext;
    }

    [HttpPost]
    [ProducesDefaultResponseType(typeof(CommonJsonResponse<LoginResponseModel>))]
    public async Task<CommonJsonResponse> Login(LoginRequestModel requestModel)
    {
        var user = await _userService.Find(c => c.Email == requestModel.Email).FirstOrDefaultAsync();
        if (user == null)
            return _workContext.Failed("User not found", "403");
        if (user.PasswordHash != await _authHelper.GetAuthStringHash(requestModel.AuthString))
            return _workContext.Failed("Invalid Password", "403");

        return _workContext.Successed(new LoginResponseModel()
        {
            Token = _authHelper.Create(user.Id)
        });
    }

    
}