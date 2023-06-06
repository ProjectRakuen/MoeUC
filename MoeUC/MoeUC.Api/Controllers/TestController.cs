using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoeUC.Core.Infrastructure.Data;
using MoeUC.Service.ServiceBase;

namespace MoeUC.Api.Controllers;

public class TestController : BaseApiController
{
    private readonly MoeDbContext _moeDbContext;
    public TestController(MoeDbContext moeDbContext)
    {
        _moeDbContext = moeDbContext;
    }
#if DEBUG
    [HttpGet]
    [Authorize()]
    public string GetCreationSql()
    {
        return _moeDbContext.GenerateCreateScript();
    }
#endif
}