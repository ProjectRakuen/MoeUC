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

    [StringLength(128)]
    public string? UserGuid { get; set; }
}