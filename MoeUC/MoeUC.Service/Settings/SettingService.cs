using Microsoft.EntityFrameworkCore;
using MoeUC.Core.Domain;
using MoeUC.Core.Infrastructure.Data;
using MoeUC.Service.ServiceBase;
using MoeUC.Service.ServiceBase.Caching;
using MoeUC.Service.ServiceBase.Models;

namespace MoeUC.Service.Settings;

public class SettingService : BaseService<Setting>
{
    private readonly MemoryCacheManager _memoryCacheManager;
    public SettingService(IRepository<Setting> repository, WorkContext workContext, MemoryCacheManager memoryCacheManager) : base(repository, workContext)
    {
        _memoryCacheManager = memoryCacheManager;
    }

    private readonly CacheKey _settingsCacheKey = new CacheKey("MoeUC:Setting", TimeSpan.FromHours(1));

    public async Task<string?> GetSetting(string key)
    {
        return await _memoryCacheManager.GetAsync(PrepareKey(key), async () =>
        {
            var entity = await Find(c => c.Key == key).FirstAsync();
            return entity.Value;
        });
    }

    private CacheKey PrepareKey(string settingsKey)
    {
        return _settingsCacheKey.WithSuffix(settingsKey);
    }
}