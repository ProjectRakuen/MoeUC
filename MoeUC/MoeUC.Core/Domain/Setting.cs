using System.ComponentModel.DataAnnotations;
using MoeUC.Core.Infrastructure.Data;

namespace MoeUC.Core.Domain;

public class Setting : BaseEntity
{
    [StringLength(128)]
    public string Key { get; set; } = null!;

    [StringLength(2048)]
    public string? Value { get; set; }
}