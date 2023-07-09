namespace MoeUC.Api.Models;

public class UserRegisterResponseModel
{
    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string GitHubHtmlUrl { get; set; } = null!;
}