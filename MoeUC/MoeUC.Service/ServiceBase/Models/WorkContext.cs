using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MoeUC.Core.Infrastructure.Dependency;
using MoeUC.Service.Auth;

namespace MoeUC.Service.ServiceBase.Models;

public class WorkContext : IScoped
{
    private readonly HttpContext? _httpContext;
    private readonly ILogger<WorkContext> _logger;
    private readonly JwtHelper _jwtHelper;
    private const string RequestStatisticsName = "Base:RequestStatistic";
    private const string AuthTokenName = "AuthToken";
    public readonly RequestStatisticModel RequestStatistic;

    public UserInfoModel CurrentUserInfo
    {
        get
        {
            var token = _httpContext?.Request?.Headers?[AuthTokenName];
            if (token.HasValue)
            {
                var userId = _jwtHelper.GetUserIdFromToken(token.Value.FirstOrDefault());
            }
            return new UserInfoModel();
        }
    }

    public WorkContext(IHttpContextAccessor httpContextAccessor, ILogger<WorkContext> logger, JwtHelper jwtHelper)
    {
        this._httpContext = httpContextAccessor.HttpContext;
        _logger = logger;
        _jwtHelper = jwtHelper;
        if (_httpContext != null && !_httpContext.Items.ContainsKey(RequestStatisticsName))
        {
            this.RequestStatistic = new RequestStatisticModel();
            _httpContext.Items.Add(RequestStatisticsName, RequestStatistic);
        }
        else
        {
            this.RequestStatistic = (RequestStatisticModel?)(_httpContext?.Items[RequestStatisticsName]) ?? new RequestStatisticModel();
        }
    }

    public CommonJsonResponse Successed(object? data = default, string? message = null)
    {
        return new CommonJsonResponse()
        {
            Success = true,
            Data = data,
            Message = message,
            Statistics = (RequestStatisticModel?)_httpContext?.Items[RequestStatisticsName]
        };
    }

    public CommonJsonResponse Failed(string? message = null, string? errorCode = null)
    {
        return new CommonJsonResponse()
        {
            Success = false,
            ErrorCode = errorCode,
            Message = message
        };
    }
}