using MoeUC.Core.Domain;
using MoeUC.Core.Infrastructure.Data;
using MoeUC.Service.ServiceBase;

namespace MoeUC.Service.User;

public class MoeUserService : BaseService<MoeUser>
{
    public MoeUserService(IRepository<MoeUser> repository) : base(repository)
    {
    }
}