using MoeUC.Core.Domain;
using MoeUC.Core.Infrastructure.Data;
using MoeUC.Service.Auth;
using MoeUC.Service.ServiceBase;
using MoeUC.Service.ServiceBase.Caching;
using MoeUC.Service.ServiceBase.Models;

namespace MoeUC.Service.User;

public class MoeUserService : BaseService<MoeUser>
{
    private readonly AuthHelper _authHelper;
    private readonly RedisCacheManager _redisCacheManager;


    public MoeUserService(IRepository<MoeUser> repository, WorkContext workContext, AuthHelper authHelper, RedisCacheManager redisCacheManager) : base(repository, workContext)
    {
        _authHelper = authHelper;
        _redisCacheManager = redisCacheManager;
    }

    private readonly CacheKey UserTokenCacheKey = new CacheKey("MoeUC:User:Token", TimeSpan.FromHours(5));

    public async Task<MoeUser> Register(string email, string password)
    {
        var hash = await _authHelper.GetAuthStringHash(password);

        var newUser = new MoeUser()
        {
            Email = email,
            PasswordHash = hash
        };

        await InsertAsync(newUser);

        return newUser;
    }

    public string GetUserToken(int userId)
    {
        var key = GetUserTokenCacheKey(userId);
        var token = _redisCacheManager.Get(key, () => _authHelper.CreateToken(userId));

        if (string.IsNullOrWhiteSpace(token))
            throw new Exception("Null Token");

        return token;
    }

    public CacheKey GetUserTokenCacheKey(int userId)
    {
        return UserTokenCacheKey.WithSuffix(userId);
    }
}