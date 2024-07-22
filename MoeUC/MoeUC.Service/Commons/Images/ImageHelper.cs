using System.Drawing;
using Microsoft.Extensions.Logging;
using MoeUC.Core.Infrastructure.Dependency;
using MoeUC.Service.Settings;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

namespace MoeUC.Service.Commons.Images;

public class ImageHelper : IScoped
{
    private readonly ILogger<ImageHelper> _logger;
    private readonly SettingService _settingService;
    private readonly IHttpClientFactory _httpClientFactory;

    public ImageHelper(ILogger<ImageHelper> logger, SettingService settingService, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _settingService = settingService;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> SaveWebImage(string imageUrl, SaveImageOptions options)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientNames.Default);
            var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

            return await SaveImage(imageBytes, options);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Get web image fail! URL:{imageUrl}");
            throw;
        }
    }

    public async Task<string> SaveImage(byte[] imageBytes, SaveImageOptions options)
    {
        var imageName = options.Name;

        var path = await GetPath(imageName, options.SubDir);
        using var stream = new MemoryStream(imageBytes);
        var image = await Image.LoadAsync(stream);

        if (options.Height != null && options.Width != null)
            image.Mutate(c => c.Resize(options.Width.Value, options.Height.Value));

        await image.SaveAsJpegAsync(path);

        var baseUri = new Uri((await _settingService.GetSetting(SettingNames.ImageBaseUri))!);
        return new Uri(baseUri, imageName).ToString();
    }

    private async Task<string> GetPath(string name, string? subDir = null)
    {
        var basePath = await _settingService.GetSetting(SettingNames.ImageStorageBasePath);

        var result = basePath!;
        if (!string.IsNullOrWhiteSpace(subDir))
            result = Path.Combine(basePath!, subDir);

        return Path.Combine(result, name);
    }
}