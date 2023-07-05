using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MoeUC.Core.Domain;
using MoeUC.Core.Infrastructure.Dependency;
using MoeUC.Service.Commons.Images;
using MoeUC.Service.Settings;
using MoeUC.Service.User;
using Octokit;

namespace MoeUC.Service.ThirdParties;

public class GitHubService : IScoped
{
    private readonly ILogger<GitHubService> _logger;
    private readonly SettingService _settingService;
    private readonly MoeUserService _userService;
    private readonly ImageHelper _imageHelper;

    public GitHubService(ILogger<GitHubService> logger, SettingService settingService, MoeUserService userService, ImageHelper imageHelper)
    {
        _logger = logger;
        _settingService = settingService;
        _userService = userService;
        _imageHelper = imageHelper;
    }

    public async Task<MoeUser> InitUserByGitHub(string code)
    {
        var client =
            new GitHubClient(new ProductHeaderValue(await _settingService.GetSetting(SettingNames.GitHubProductHeaderValue)));
        var clientId = await _settingService.GetSetting(SettingNames.GitHubClientId);
        var clientSecret = await _settingService.GetSetting(SettingNames.GitHubClientSecret);

        var request = new OauthTokenRequest(clientId, clientSecret, code);
        var token = await client.Oauth.CreateAccessToken(request);

        if (string.IsNullOrWhiteSpace(token.AccessToken))
            throw new Exception($"Get Github token failed, error: {token.Error}, {token.ErrorDescription}");

        client.Credentials = new Credentials(token.AccessToken);
        var gitHubUser = await client.User.Current();

        var moeUser = new MoeUser()
        {
            Avatar = gitHubUser.AvatarUrl,
            Description = gitHubUser.Bio,
            Email = gitHubUser.Email,
            GitHubHtmlUrl = gitHubUser.HtmlUrl,
            GitHubId = gitHubUser.Id,
            UserName = gitHubUser.Name
        };

        await _userService.InsertAsync(moeUser);

        return moeUser;
    }
}