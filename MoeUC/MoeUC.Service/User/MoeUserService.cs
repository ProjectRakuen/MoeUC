using MoeUC.Core.Domain;
using MoeUC.Core.Infrastructure.Data;
using MoeUC.Service.ServiceBase;
using MoeUC.Service.ServiceBase.Models;

namespace MoeUC.Service.User;

public class MoeUserService : BaseService<MoeUser>
{
    public MoeUserService(IRepository<MoeUser> repository, WorkContext workContext) : base(repository, workContext)
    {
    }
}