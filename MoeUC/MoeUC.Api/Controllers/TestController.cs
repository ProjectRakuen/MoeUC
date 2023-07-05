using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoeUC.Core.Infrastructure.Data;
using MoeUC.Service.ServiceBase;
using MoeUC.Service.ServiceBase.Models;

namespace MoeUC.Api.Controllers;

public class TestController : BaseApiController
{
    private readonly MoeDbContext _moeDbContext;

    public TestController(WorkContext workContext, MoeDbContext moeDbContext) : base(workContext)
    {
        _moeDbContext = moeDbContext;
    }

    
#if DEBUG
    [HttpGet]
    public string GetCreationSql()
    {
        return _moeDbContext.GenerateCreateScript();
    }
#endif
    }