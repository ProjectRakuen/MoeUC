using System.ComponentModel.DataAnnotations;
using MoeUC.Core.Infrastructure.Data;

namespace MoeUC.Core.Domain;

public class MoeUser : BaseEntity
{
    [StringLength(64)]
    public string? UserName { get; set; }

    [StringLength(64)]
    public string? Email { get; set; }

    [StringLength(512)]
    public string? PasswordHash { get; set; }

    [StringLength(2048)]
    public string? Description { get; set; }

    [StringLength(1024)]
    public string? Avatar { get; set; }

    public int GitHubId { get; set; }

    [StringLength(1024)]
    public string? GitHubHtmlUrl { get; set; }
}