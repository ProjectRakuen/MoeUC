using Microsoft.AspNetCore.Mvc;
using MoeUC.Service.ServiceBase.Models;

namespace MoeUC.Service.ServiceBase;

[Route("[controller]/[action]")]
public class BaseApiController : Controller
{
    protected readonly WorkContext _workContext;

    public BaseApiController(WorkContext workContext)
    {
        this._workContext = workContext;
    }
}